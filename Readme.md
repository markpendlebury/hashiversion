# Hashiversion

[![elesoft](https://circleci.com/gh/markpendlebury/hashiversion.svg?style=shield)](https://circleci.com/gh/markpendlebury/hashiversion)


[Hashiversion](https://hashiversion.elesoft.co.uk) is a web api written in c# designed to provide easy endpoints to query the latest versions of software from [Hashicorp](https://hashicorp.com)


# How it works
It works by making a request to hashicorps github tags page, filtering out unwanted tags (such as beta/alpha releases etc) and returns what it thinks is the latest stable release of any given hashicorp product.

# Usage

Using curl, get the latest terraform version:

`TERRAFORM_VERSION=$(curl -s https://hashiversion.elesoft.co.uk/terraform)`

Using curl, get the latest packer version: 

`PACKER_VERSION=$(curl -s https://hashiversion.elesoft.co.uk/packer)`


