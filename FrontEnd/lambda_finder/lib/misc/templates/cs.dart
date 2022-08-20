import 'package:hooks_riverpod/hooks_riverpod.dart';

final csTemplateProvider = Provider((_) => """
public class Lambda {

    /*
    * This is the main function that will be executed.
    * It must have parameters and the return type should not be void.
    * It should run for any input in ~2 seconds.
    * The name must be "lambda".
    */
    public int lambda(int x) {
        // Write some code here
        return x;
    }

    /*
    * This is the lambda function inverse.
    * This function should be implemented if the function is invertible.
    * It should take the output of the lambda function and map it to the input.
    * Uncomment the following lines only if you decide to implement it.
    */
    //public int lambdaInverse(int x) {
    //    // Write some code here
    //    return x;
    //}
}
""");
