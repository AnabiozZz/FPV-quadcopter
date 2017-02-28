#include <string.h>
#include <stdlib.h>
#include <stdio.h>
#include <unistd.h>
#include <fcntl.h>
#include <termios.h>
#include<arpa/inet.h>
#include<sys/socket.h>

#define BUFLEN 2  //Max length of buffer

void die(char *s)
{
	perror(s);
	exit(1);
}


int main(int argc, char** argv)
{
	struct sockaddr_in si_me, si_other;

	int s, i, slen = sizeof(si_other), recv_len;
	char buf[BUFLEN];

	struct termios tio;
	int tty_fd;

	 char c[BUFLEN];

	printf("Please start with %s /dev/ttyS1 (for example)\n", argv[0]);

	if ((s = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP)) == -1)
	{
		die("socket");
	}

	memset((char *)&si_me, 0, sizeof(si_me));

	si_me.sin_family = AF_INET;
	si_me.sin_port = htons(23470);
	si_me.sin_addr.s_addr = htonl(INADDR_ANY);

	//bind socket to port
	if (bind(s, (struct sockaddr*)&si_me, sizeof(si_me)) == -1)
	{
		die("bind");
	}


	memset(&tio, 0, sizeof(tio));
	tio.c_iflag = 0;
	tio.c_oflag = 0;
	tio.c_cflag = CS8 | CREAD | CLOCAL;           // 8n1, see termios.h for more information
	tio.c_lflag = 0;
	tio.c_cc[VMIN] = 1;
	tio.c_cc[VTIME] = 5;

	tty_fd = open(argv[1], O_RDWR | O_NONBLOCK);
	if (tty_fd == -1)
	{
		printf("failed to open port\n");
	}
	cfsetospeed(&tio, B9600);            // 115200 baud
	cfsetispeed(&tio, B9600);            // 115200 baud

	tcsetattr(tty_fd, TCSANOW, &tio);
	while (1)
	{
		if ((recv_len = recvfrom(s, buf, BUFLEN, 0, (struct sockaddr *) &si_other, &slen)) == 2)
		{      if ((buf[0] == 0x01) && (buf[1] == 0xFF)) 
			{break;}
			printf("recieved %d %d\n",buf[0],buf[1]);
			write(tty_fd, &buf, 2);
		}


		if (read(tty_fd, &c, 2)>0)
		{
			printf("reading %d %d\n",c[0],c[1]);
			//if (sendto(s, c, 1, 0, (struct sockaddr*) &si_other, slen) == -1)
			//{
			//	die("sendto()");
			//}
			// if new data is available on the serial port, print it out
		}
	}
	close(s);
	close(tty_fd);

	return EXIT_SUCCESS;
}
