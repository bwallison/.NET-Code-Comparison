function checkFileType() {
    var markerExtension = document.getElementById("markerFile").value.match(/\.(.+)$/)[1];

    switch (markerExtension) {
        case 'c':
        case 'cs':
        case 'cpp':
            if ((document.getElementById("markerFile").value != ' ') && (document.getElementById("studentFile").value != ' ')) {
                var studentExtension = document.getElementById("studentFile").value.match(/\.(.+)$/)[1];
                switch (studentExtension) {
                    case 'c':
                    case 'cs':
                    case 'cpp':
                        document.getElementById("compareFile").disabled = false;
                    break;
                    default:
                        alert('One or more upload files are not valid file types, please select a valid file.\n .c, .cs, .cpp are the current supported filetypes.');
                        document.getElementById("compareFile").disabled = true;
                        this.value = '';
                }
            }
            else {
                document.getElementById("compareFile").disabled = true;
            }
        break;
        default:
            alert('One or more upload files are not valid file types, please select a valid file.\n .c, .cs, .cpp are the current supported filetypes.');
            document.getElementById("compareFile").disabled = true;
            this.value = '';
    }
}
