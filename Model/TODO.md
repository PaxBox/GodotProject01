# A list of tasks to get this thing started

## Outline

- Godot
- Unit tests
- 

Learn how to pronouce Godot: https://www.youtube.com/watch?v=ZjQdF4wKgXU and tell Darren


## Description

Write a program which reads a JSON file containing the structure described in this project.

The function to read the file should use file watcher which simple polls the last access time of the file. If the file has changed, it should be reloaded.

The program should then programatically build a 3D scene in Godot, using the data from the JSON file.

## Thinking ahead

- Polling Prometheus for data about the devices
- Polling logstash for data about the devices
- Showing "real-time" as in polled about every 10 to 15 seconds data showing use (for example disk performance and remaining capacity) per disk and usage of network ports.
- Showing errors and such from logstash and then making cool visuals to know what piece is broken
- Show network interfaces on switches and basic port usage (up/down/unused) performance (gb/s)


## Darren's responsbilities

- Create accounts on the test lab
- Forward logstash and prometheus information to the test lab

