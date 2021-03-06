.. csharp-gradecalculator documentation master file, created by
   sphinx-quickstart on Mon May 15 22:59:00 2017.
   You can adapt this file completely to your liking, but it should at least
   contain the root `toctree` directive.

Welcome to csharp-gradecalculator's documentation!
==================================================

.. toctree::
   :maxdepth: 3
   :caption: Table of contents:

csharp-gradecalculator is a grade calculator for BTEC qualifications,
written in C# and using .NET Core. It calculates and looks up BTEC
points and grades, in addition to UCAS points.

.. image:: images/screenshot.png

Features
--------

-  Uses a SQLite database
-  Calculates BTEC points from number of passes, merits, and
   distinctions
-  Looks up BTEC grades from BTEC points
-  Looks up UCAS points from BTEC grades

Installation
------------

Unfortunately the software does not have any pre-compiled binaries at
the moment, so it is necessary to compile the program from its source
code.

Installation from source code
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

csharp-gradecalculator supports Windows, macOS, and Linux.

Windows
^^^^^^^

#. Ensure that the .NET Core runtime is installed:

   a. Download the `.NET Core runtime`_ installer.
   b. Install the .NET core runtime by running the installer and following the steps (administrator access is needed for this).

#. `Download the software`_ from GitHub.
#. Extract the ZIP file that the software was downloaded in with a ZIP file extractor of your choice, such as 7Zip or PeaZip. The built-in Windows Explorer extractor can also be used.
#. Press :kbd:`Shift` + right click in the folder that the software was extracted into.
#. Select :guilabel:`&Open command window here`. A command window should open.
#. Type :command:`dotnet restore`, then press :kbd:`Enter` in the command window to download the necessary dependencies.
#. Type :command:`dotnet build`, then press :kbd:`Enter` in the command window to build the application from its source code. 
#. Type :command:`dotnet run`, then press :kbd:`Enter` in the command window to run the application.
#. Use the application as normal.

Contributing
------------

-  Issue tracker: https://github.com/tom29739/csharp-gradecalculator/issues
-  Source code: https://github.com/tom29739/csharp-gradecalculator

Support
-------

If you are having issues with csharp-gradecalculator, please let us know using the `issue tracker`_.

Licence
-------

csharp-gradecalculator is licenced under the MIT License.

Links
-----

* :ref:`search`

.. _.NET Core runtime: https://www.microsoft.com/net/download/core#/sdk
.. _Download the software: https://github.com/tom29739/csharp-gradecalculator/archive/master.zip
.. _issue tracker: https://github.com/tom29739/csharp-gradecalculator/issues
