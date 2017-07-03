SET "SRC=%~f1"
SET "DEST=%~f2"
robocopy "%SRC%" "%DEST%" /XF *.vssscc *.vsmdi *.suo *.vspscc *.user /XD /A-:R *bin *obj *packages *ClientBin *_ReSharper.* *.nuget /E
