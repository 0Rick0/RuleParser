# Rule Parser
This is a C# library build to parse human readable rules like:
```
SEND EMAIL TO steve@jobs.com WHEN DATE GREATER THEN 2017 AND CARS CAN FLY
PRINT MESSAGE WHEN USER NAME EQUAL TO "somthing"
```

>##### NOTE
>Nor README nor the application are done. This will take some time!
>I know the code isn't the best, it began as a test but turend out to be a cool project. It will be better eventually.

## Strenghts
The strenght of this library is that the user is suposed to implement the features it supports.
The user must implement an interface called ITestProvider which tell's the parser if an statement evaluates to True or False.
It only knows about `WHEN`, `AND`, `OR` and `()`(Not implemented inside strings yet so watch out!).

## Current version
This version has a test application attathed called TrueFalseTester which has only has one class that imlements the ITestProvider interface.
This class evaluates `BOOL TRUE` to True and `BOOL FALSE` and others to False. Since the user is suposed to provide the test provider it doesn't matter it is this simple.
But ofcourse a more advanced test will come sooner or later.

## Future
I wish to implement the following features:

 - [ ] Multiple TestProviders
 - [ ] Dynamic and Static test providers. e.g. some that are always presend and some that are only present per execution.
 - [ ] caching of static test provider results
 - [ ] Proper in-quote support for brackets.
 - [ ] `WHEN NOT` keyword
 - [ ] Helper class(es) for logic parser like equals etc.
 - [ ] Predefined parsers for 'simple' common stuff.
 - [ ] Rule file loader
 - [ ] Rule database loader
 - [ ] Rules container for executing multiple rules.
 - [ ] Lower case support.
 - [ ] Better performance/more precompiling