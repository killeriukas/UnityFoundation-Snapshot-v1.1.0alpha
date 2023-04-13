using System;
using TMI.Helper;

namespace TMI {

    public static class Assert {

        public static void IsNull(object testObject, string errorMessage) {
            if(testObject == null) {
                throw new ArgumentNullException(errorMessage);
            }
        }

		public static void IsNotNull(object testObject, string errorMessage) {
			if(testObject != null) {
				throw new ArgumentException(errorMessage);
			}
		}

		public static void IsTrue<TExceptionType>(bool isTrue, string errorMessage) where TExceptionType : Exception {
			if(isTrue) {
				TExceptionType exception = GeneralHelper.CreateInstance<TExceptionType>(errorMessage);
				throw exception;
			}
		}

		public static void IsFalse<TExceptionType>(bool isFalse, string errorMessage) where TExceptionType : Exception {
			bool isTrue = !isFalse;
			IsTrue<TExceptionType>(isTrue, errorMessage);
		}

    }


}