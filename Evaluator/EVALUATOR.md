# The evaluator

The only place in the project where unsafe user code is evaluated. 
It will be completly sandboxed.

## The external interface
    EVALUATE
        string language
        string code
        [string] inputs
        string path_to_evaluators
    -> String result (code: 0) or error (code: -1)
    
    VERIFY
        string language
        string code
        string path_to_evaluators
    -> OK(code: 0) or ERROR(code: -1)

## Structure
    C++ Driver Program
        C# Evaluator Program
        Java Evaluator Program
        Other Evaluator Programs

The C++ Driver handles invoking the correct evaluator for the function.

## The internal interface
A evaluator must have 2 functions.

    Evaluate
        string code
        [string] input
    -> [string] outputs or error

    Verify
        string code
    -> OK or ERROR

A language evaluator must also support generating random values for the Verify interface.

