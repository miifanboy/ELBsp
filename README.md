# ELBsp
Software to add event listener to source engine .bsp files without recompiling them.

# Usage

(screenshot1)

-Drag and drop your bsp file.

Wow magic now you have a bsp called "yourbsp_elbsp_0.bsp" which has event listener in it.

> Note: Default save location is the input bsp location
# Features

Did you know that you can also click open to select your bsp file ?

now you know

Custom Save: It will ask you for your save location when you drag and drop or open your file.

Copy Navmesh: It will search for yourbsp.nav and it will copy it to save location as newbspname.nav

# How does it work?

This software consists of two different programs

### 1.ELBsp_CLI.exe

This program is written in python and it takes two command arguments:

> -i "inputpath" -o "outputpath"

It takes the input bsp and adds event listener entity to it then saves it to output path.

### 2.ELBsp.exe

This program is written in C# and it is used as a gui. It has ELBsp_CLI.exe included with it. 

When you run it , it extracts ELBsp_CLI.exe to your temp folder.

After you select your bsp file ,it runs ELBsp_CLI.exe with the needed command arguments. 

ELBsp_CLI.exe gets automatically deleted from your temp folder when you close the program.
