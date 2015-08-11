# nunit-testcaseattribute-externaldata
Ever wondered if your could somehow reliably know the name of the executing test, decorated with TestCaseSourceAttribute. Here's how to do it in .NET 4.5+


The sample JsonTestCaseSource even supports setting up, a template TestData, so that some properties for tests you just put inot the template.json file, and the outcome JSON is a product of merging the template and the test specific data.


The whle ide:
I use the [CallerMemberName] attribute introduced in .NET 4.5 o get the name of the test method for which this attribute was defined and which is currently executing.

Before this, it was hard and unpredictable to know the name of the test methodfor which the attrbute was defined.
It works even with multiple test cases for a single method.
You just have to build the project and all the test cases will show up in the test explorer.
