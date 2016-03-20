CC~=fsharpc

TGT = brainfuck.exe

SRC = \
	brainfuck.fs

ARGS = \
	helloworld.bf

.PHONY: run

run: $(TGT)
	@mono $(TGT) $(ARGS)

build: $(TGT)

$(TGT): $(SRC)
	@fsharpc $(SRC)
