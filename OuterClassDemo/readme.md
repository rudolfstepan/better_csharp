This shows the concept of outer and inner classes in c#
when the inner class should be able to access outer class members like in java.

With this approach the outer class instance must not be used as a ctr member when initializing the inner class.
This helps to minimize ctr parameters.
