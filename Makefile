CC~=fsharpc

TGT = brainfuck.exe

SRC = \
	brainfuck.fs

ARGS = \
	brainfuck.bf

.PHONY: run

run: $(TGT)
	@mono $(TGT) $(ARGS)

build: $(TGT)

$(TGT): $(SRC)
	@fsharpc $(SRC)
