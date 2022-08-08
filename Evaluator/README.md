# Compile
To compile cd into the build folder and run the following command:

```
cmake ../ -DCMAKE_BUILD_TYPE=Debug -G "Unix Makefiles"
```

Now to run tests just do:
``` 
./tst/EvaluatorDriver_tst
```

The actual app is located at 
```
build/src/EvaluatorDriver_run
```