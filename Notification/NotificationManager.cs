using System;
using System.Collections.Generic;
using TMI.Core;
using TMI.LogManagement;

namespace TMI.Notification {

    [Loggable]
	sealed public class NotificationManager : BaseManager, INotificationManager {

		private interface ITrigger {
			void Callback(INotification notification);
			bool IsEqual(INotificationListener listener);
            Type GetListenerType();
		}

        private abstract class BaseNotificationMapping<TNotification> : ITrigger where TNotification : INotification {
			protected readonly Action<TNotification> callback;
			private readonly INotificationListener listener;

			protected BaseNotificationMapping(INotificationListener listener, Action<TNotification> callback) {
				this.listener = listener;
				this.callback = callback;
			}

			public bool IsEqual(INotificationListener listener) {
				return this.listener == listener;
			}

            public abstract void Callback(INotification notification);

            public Type GetListenerType() {
                return listener.GetType();
            }
		}

        private class NotificationMapping<TNotification> : BaseNotificationMapping<TNotification> where TNotification : INotification {

            protected NotificationMapping(INotificationListener listener, Action<TNotification> callback) : base(listener, callback) {
                
            }

            public override void Callback(INotification notification) {
                TNotification exactNotification = (TNotification)notification;
                callback(exactNotification);
            }

            public static ITrigger Create(INotificationListener listener, Action<TNotification> callback) {
                return new NotificationMapping<TNotification>(listener, callback);
            }

        }

        private class PredicateNotificationMapping<TNotification, TPredicate> : BaseNotificationMapping<TNotification> where TNotification : BasePredicateNotification where TPredicate : BasePredicateNotification {
            private readonly Func<TPredicate, bool> predicate;

            protected PredicateNotificationMapping(INotificationListener listener, Action<TNotification> callback, Func<TPredicate, bool> predicate) : base(listener, callback) {
                this.predicate = predicate;
            }

            public override void Callback(INotification notification) {
                TPredicate predicateNotification = (TPredicate)notification;
                bool hasPredicatePassed = predicate(predicateNotification);

                if(hasPredicatePassed) {
                    TNotification exactNotification = (TNotification)notification;
                    callback(exactNotification);
                }
            }

            public static ITrigger Create(INotificationListener listener, Action<TNotification> callback, Func<TPredicate, bool> predicate) {
                return new PredicateNotificationMapping<TNotification, TPredicate>(listener, callback, predicate);
            }
        }

        private Dictionary<INotificationListener, List<Type>> notificationTypeByListener = new Dictionary<INotificationListener, List<Type>>();
		private Dictionary<Type, LinkedList<ITrigger>> triggerByNotificationType = new Dictionary<Type, LinkedList<ITrigger>>();

        private LinkedList<ITrigger> FindTriggerListByNotificationType(Type notificationType) {
            LinkedList<ITrigger> foundTriggerList = null;
            if(!triggerByNotificationType.TryGetValue(notificationType, out foundTriggerList)) {
                foundTriggerList = new LinkedList<ITrigger>();
                triggerByNotificationType.Add(notificationType, foundTriggerList);
            }

            return foundTriggerList;
        }

        private List<Type> FindTypeListByListener(INotificationListener listener) {
            List<Type> foundTypeList = null;
            if(!notificationTypeByListener.TryGetValue(listener, out foundTypeList)) {
                foundTypeList = new List<Type>();
                notificationTypeByListener.Add(listener, foundTypeList);
            }

            return foundTypeList;
        }

		public void Listen<TNotification>(INotificationListener listener, Action<TNotification> callback) where TNotification : INotification {
			Assert.IsNull(listener, "Listener cannot be null!");

            Type notificationType = typeof(TNotification);
            Type listenerType = listener.GetType();
            Logging.Log(this, "Listen<TNotification> of type [{0}] for a listener type [{1}].", notificationType.Name, listenerType.Name);

            LinkedList<ITrigger> foundTriggerList = FindTriggerListByNotificationType(notificationType);

			ITrigger trigger = NotificationMapping<TNotification>.Create(listener, callback);

			//TODO: add validation for double addition of the same listener
			foundTriggerList.AddFirst(trigger);

            List<Type> foundTypeList = FindTypeListByListener(listener);

            ////TODO: THIS ONE SHOULD TRIGGER THE EXCEPTION, BECAUSE STORY IS BEING LISTENED MORE THAN ONCE BY THE SAME LISTENER!!!!!!
            //Assert.IsTrue<InvalidOperationException>(foundTypeList.Contains(notificationType),
            //    "One listener cannot listen to the same notification types more than once. Listener: " + listener.GetType().Name + " Notification: " + notificationType);

			foundTypeList.Add(notificationType);
		}

        public void Listen<TNotification>(INotificationListener listener, Action<TNotification> callback, Func<TNotification, bool> predicate) where TNotification : BasePredicateNotification {
            Listen<TNotification, TNotification>(listener, callback, predicate);
        }

        public void Listen<TNotification, TPredicate>(INotificationListener listener, Action<TNotification> callback, Func<TPredicate, bool> predicate) where TNotification : BasePredicateNotification where TPredicate : BasePredicateNotification {
            Assert.IsNull(listener, "Listener cannot be null!");

            Type notificationType = typeof(TNotification);
            Type listenerType = listener.GetType();
            Logging.Log(this, "Listen<TNotification, TPredicate> of type [{0}] for a listener type [{1}].", notificationType.Name, listenerType.Name);

            LinkedList<ITrigger> foundTriggerList = FindTriggerListByNotificationType(notificationType);

            ITrigger trigger = PredicateNotificationMapping<TNotification, TPredicate>.Create(listener, callback, predicate);

            //TODO: add validation for double addition of the same listener
            foundTriggerList.AddFirst(trigger);

            List<Type> foundTypeList = FindTypeListByListener(listener);

            ////TODO: THIS ONE SHOULD TRIGGER THE EXCEPTION, BECAUSE STORY IS BEING LISTENED MORE THAN ONCE BY THE SAME LISTENER!!!!!!
            //Assert.IsTrue<InvalidOperationException>(foundTypeList.Contains(notificationType),
            //    "One listener cannot listen to the same notification types more than once. Listener: " + listener.GetType().Name + " Notification: " + notificationType);

            foundTypeList.Add(notificationType);
        }

        public void Trigger(INotification notification) {
            Type notificationType = notification.GetType();
            Logging.Log(this, "Triggering notification of type [{0}]", notificationType.Name);

            LinkedList<ITrigger> foundList = null;
			if(triggerByNotificationType.TryGetValue(notificationType, out foundList)) {
                LinkedListNode<ITrigger> triggerNodeList = foundList.First;
                while(triggerNodeList != null) {
                    LinkedListNode<ITrigger> nextTriggerNode = triggerNodeList.Next;
                    triggerNodeList.Value.Callback(notification);
                    triggerNodeList = nextTriggerNode;
                }
            }
        }

		public void StopListen<TNotification>(INotificationListener listener) where TNotification : INotification {
			Assert.IsNull(listener, "Listener cannot be null!");

			Type notificationType = typeof(TNotification);

            StopListen(notificationType, listener);
		}

		private void StopListen(Type notificationType, INotificationListener listener) {
            Type listenerType = listener.GetType();
            Logging.Log(this, "StopListen() notification of type [{0}] for a listener type [{1}].", notificationType.Name, listenerType.Name);

            List<Type> notificationTypeList;
			if(notificationTypeByListener.TryGetValue(listener, out notificationTypeList)) {
				
				for(int i = 0; i < notificationTypeList.Count; ++i) {
					if(notificationTypeList[i] == notificationType) {
						notificationTypeList.RemoveAt(i);
						break;
					}
				}

				if(0 == notificationTypeList.Count) {
					notificationTypeByListener.Remove(listener);
				}

				LinkedList<ITrigger> foundListenerList = triggerByNotificationType[notificationType];
                LinkedListNode<ITrigger> triggerNode = foundListenerList.First;
                while(triggerNode != null) {
                    LinkedListNode<ITrigger> nextTriggerNode = triggerNode.Next;
                    if(triggerNode.Value.IsEqual(listener)) {
                        foundListenerList.Remove(triggerNode);
                        break;
                    }
                    triggerNode = nextTriggerNode;
                }

                if(0 == foundListenerList.Count) {
					triggerByNotificationType.Remove(notificationType);
				}
			}
		}

        public void StopListenAll(INotificationListener listener) {
			Assert.IsNull(listener, "Listener cannot be null!");

            Type listenerType = listener.GetType();
            Logging.Log(this, "StopListenAll() for a listener type [{0}].", listenerType.Name);

			List<Type> notificationTypesList;
			if(notificationTypeByListener.TryGetValue(listener, out notificationTypesList)) {
				foreach(Type notificationType in notificationTypesList) {
					LinkedList<ITrigger> triggerList = triggerByNotificationType[notificationType];

                    LinkedListNode<ITrigger> triggerNode = triggerList.First;
                    while(triggerNode != null) {
                        LinkedListNode<ITrigger> nextTriggerNode = triggerNode.Next;
                        if(triggerNode.Value.IsEqual(listener)) {
                            triggerList.Remove(triggerNode);
                        }
                        triggerNode = nextTriggerNode;
                    }

                    if(0 == triggerList.Count) {
                        triggerByNotificationType.Remove(notificationType);
                    }
                }

				notificationTypeByListener.Remove(listener);
			}
		}

#if UNITY_EDITOR
        sealed public class Editor {

            private readonly NotificationManager notificationManager;

            public Editor(NotificationManager notificationManager) {
                this.notificationManager = notificationManager;
            }

            public IEnumerable<Type> GetAllRegisteredNotificationTypes() {
                return notificationManager.triggerByNotificationType.Keys;
            }

            public IEnumerable<Type> GetListenerTypesByNotificationType(Type notificationType) {
                LinkedList<ITrigger> triggerList = notificationManager.triggerByNotificationType[notificationType];

                List<Type> allListenerTypes = new List<Type>();
                foreach(ITrigger trigger in triggerList) {
                    allListenerTypes.Add(trigger.GetListenerType());
                }

                return allListenerTypes;
            }

        }
#endif

    }


}