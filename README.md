# Welcome to my devops deploy solution

This project runs exclusively from unit tests. You can open the solution in visual studio 2022. The Devops.Deploy.Tests project contains all tests. 
**logs will be dumped to a logs folder your C drive**

The dependency inversion is a bit overkill for this project and probably not in the spirit of YAGNI, but I am trying to demonstrate how you could potentrially have two or three projects implemented by a 
solution (in this case unit tests) with that project being agnostic of the dependencies, also the D in SOLID. Here we can see the solution using objects it has no idea about and those object calling a logger 
they have no idea of. The service project is in the case you might want some extension methods.

