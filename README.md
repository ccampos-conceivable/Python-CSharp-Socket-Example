Example of Socket communication between Python and C Sharp

This example shows a Python Client Socket that connect to a specific HOST and Port. For this example the IP address of the test machine is used.
To get the IP address you can use the ipconfig command (IPv4).

![image](https://github.com/ccampos-conceivable/Python-CSharp-Socket-Example/assets/162512474/4395bd8f-24c6-46c3-8d94-c4fdbc7f42cb)

In the Python file set the IP and Port

![image](https://github.com/ccampos-conceivable/Python-CSharp-Socket-Example/assets/162512474/28462207-4c7d-4fc2-8299-dcd95cb7d504)

The program open the connection and send an initial command-like message to the host, then enter into a reception cycle. During this cycle
the program receives and send back all the mesages from the host, except for the acknowledge (ACK) and the CLOSE commands. ACK is used to know
that the host has received the message from the client. CLOSE will terminate the program.

The C Sharp code is a windows tester that start listening for new connections from ANY IP Address at an specific port.

![image](https://github.com/ccampos-conceivable/Python-CSharp-Socket-Example/assets/162512474/174df4c9-268f-4637-b6ad-51a67383b1c1)

The server enter into a reception cycle where every command received is shown in the main form and an ACK command is sent back to the client.

![image](https://github.com/ccampos-conceivable/Python-CSharp-Socket-Example/assets/162512474/2b13e024-3589-4716-8be4-a987bf75591b)

The application also can send open commands to client to test the communication. Every command sent to the client will be replied back to server.

![image](https://github.com/ccampos-conceivable/Python-CSharp-Socket-Example/assets/162512474/a8949b4d-6cb5-4ab6-9e41-827d5a51d147)
