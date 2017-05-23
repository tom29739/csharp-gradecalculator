.. csharp-gradecalculator documentation master file, created by
   sphinx-quickstart on Mon May 15 22:59:00 2017.
   You can adapt this file completely to your liking, but it should at least
   contain the root `toctree` directive.

Welcome to csharp-gradecalculator's documentation!
==================================================

.. toctree::
   :maxdepth: 3
   :caption: Table of contents:
   Features
   Installation
      Installation from source code
         Windows
   Contributing
   Support
   Licence

csharp-gradecalculator is a grade calculator for BTEC qualifications,
written in C# and using .NET Core. It calculates and looks up BTEC
points and grades, in addition to UCAS points.

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
   #. Download the `https://www.microsoft.com/net/download/core#/sdk`_
   installer.
   #. Install the .NET core runtime by running the installer and following
   the steps (administrator access is needed for this).
#. `https://github.com/tom29739/csharp-gradecalculator/archive/master.zip`_ from GitHub.
#. Extract the ZIP file that the software was downloaded in with a ZIP file extractor of your choice, such as 7Zip or PeaZip. The built-in Windows Explorer extractor can also be used.
#. Press Shift and right click in the folder that the software was extracted into.
#. Select *"Open command window here"*. A command window should open.
#. Type ``dotnet restore``, then press Enter in the command window to download the necessary dependencies.
#. Type ``dotnet run``, then press Enter in the command window to run the application.
#. Use the application as normal.
#. Type ``dotnet run``, then press Enter again in the command window to run the application again.

Indices and tables
==================

* :ref:`genindex`
* :ref:`modindex`
* :ref:`search`

.. _`https://www.microsoft.com/net/download/core#/sdk`: .NET%20Core%20runtime
.. _`https://github.com/tom29739/csharp-gradecalculator/archive/master.zip`: Download%20the%20software
