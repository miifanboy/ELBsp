# ELBsp
Software to add event listener to source engine .bsp files without recompiling them.

# Usage

(screenshot1)

-Drag and drop your bsp file.

Wow magic now you have a bsp called "yourbsp_elbsp_0.bsp" which has event listener in it.

# Features

Did you know that you can also click open to select your bsp file ?

now you know

Custom Save: It will ask you for your save location when you drag and drop or open your file.

Copy Navmesh: It will search for yourbsp.nav and it will copy it to save location as newbspname.nav

# How does it work?

This software consists of two different softwares

### 1.ELBsp_CLI.exe

This software is written in python and it takes two command arguments:

> -i "inputpath" -o "outputpath"

It takes the input bsp and adds event listener to it then saves it to output path.
