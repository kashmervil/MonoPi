MonoPi
======

A Raspberry Pi GPIO manipulation library for Mono/.NET implementing libbcm2835 and wiringPi
=======
MonoPi v1.0.0.0
Copyright (c) CyrusBuilt 2013

A Raspberry Pi GPIO manipulation library for Mono/.NET implementing the wiringPi v2 library (https://github.com/WiringPi/WiringPi2-Python/tree/master/WiringPi/wiringPi), the libbcm2835 library (http://www.open.com.au/mikem/bcm2835/index.html), and the LibNativeI2C library that is part of the RPi.I2C.Net library (https://github.com/mshmelev/RPi.I2C.Net/tree/master/Lib/LibNativeI2C).  The purpose of this library is to provide the ability to read/write the GPIO pins on the Raspberry Pi, as well as interface with the onboard serial (RS232 UARTs) pins, which is an alternate function of 2 of the GPIOs along with some additional interfacing options.  This library is under development and not ready for production use.  This library has grown into a bit of a "Swiss Army Knife" given the breadth of functionality that is still growing as it is adapted to its underlying C libraries. Most of it is ready for testing however, and I'd love some feed back.
