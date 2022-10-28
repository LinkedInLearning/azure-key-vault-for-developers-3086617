# Azure Key Vault for Developers
This is the repository for the LinkedIn Learning course Azure Key Vault for Developers. The full course is available from [LinkedIn Learning][lil-course-url].

![Azure Key Vault for Developers][lil-thumbnail-url] 

Do you need to learn how to manage sensitive information for your applications and services? In this course, instructor Nertil Poci shows you how to secure sensitive information with Azure Key Vault. Nertil explains what Azure Key Vault does, then goes over creating Key Vaults, setting up your project dependencies, and securely accessing information stored in Key Vaults. He covers a variety of ways to store application secrets in Azure Key Vault, including working with Azure Key Vault, binding Key Vault secrets in .NET applications, creating secrets using the Key Vault API, and much more.  Nertil shows you how to use Azure Key Vault to generate and secure cryptographic keys, then dives into creating and managing certificates in Azure Key Vault. Plus, he highlights best practices for monitoring Azure Key Vault, including adding throttling to your API calls, caching Key Vault API calls, enabling logging, and setting up alerts.

## Instructions
This repository has branches for each of the videos in the course. You can use the branch pop up menu in github to switch to a specific branch and take a look at the course at that stage, or you can add `/tree/BRANCH_NAME` to the URL to go to the branch you want to access.

## Branches
The branches are structured to correspond to the videos in the course. The naming convention is `CHAPTER#_MOVIE#`. As an example, the branch named `02_03` corresponds to the second chapter and the third video in that chapter. 
Some branches will have a beginning and an end state. These are marked with the letters `b` for "beginning" and `e` for "end". The `b` branch contains the code as it is at the beginning of the movie. The `e` branch contains the code as it is at the end of the movie. The `main` branch holds the final state of the code when in the course.

When switching from one exercise files branch to the next after making changes to the files, you may get a message like this:

    error: Your local changes to the following files would be overwritten by checkout:        [files]
    Please commit your changes or stash them before you switch branches.
    Aborting

To resolve this issue:
	
    Add changes to git using this command: git add .
	Commit changes using this command: git commit -m "some message"

## Installing
1. To use these exercise files, you must have the following installed:
	- [list of requirements for course]
2. Clone this repository into your local machine using the terminal (Mac), CMD (Windows), or a GUI tool like SourceTree.
3. [Course-specific instructions]


### Instructor

Nertil Poci 
                            
Software Developer

                            

Check out my other courses on [LinkedIn Learning](https://www.linkedin.com/learning/instructors/nertil-poci).

[lil-course-url]: https://www.linkedin.com/learning/azure-key-vault-for-developers?dApp=59033956
[lil-thumbnail-url]: https://cdn.lynda.com/course/3086617/3086617-1666634981132-16x9.jpg
