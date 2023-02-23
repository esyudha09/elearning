/* ========================================================================= */
/*	Preloader
/* ========================================================================= */

jQuery(window).load(function () {

    $("#preloader").fadeOut("slow");

});

function SetInputNumberOnly(event) {
    if ((event.keyCode >= 48 && event.keyCode <= 57) ||
        (event.keyCode >= 96 && event.keyCode <= 105) ||
         event.keyCode === 8 ||
         event.keyCode === 9 ||
         event.keyCode === 17 ||
         event.keyCode === 46 ||
         event.keyCode === 37 ||
         event.keyCode === 38 ||
         event.keyCode === 39 ||
         event.keyCode === 40 ||
         event.keyCode === 35 ||
         event.keyCode === 36) {
        return true;
    }
    else {
        return false;
    }
}

function SetInputNumberDecimalOnly(event) {
    if ((event.keyCode >= 48 && event.keyCode <= 57) ||
        (event.keyCode >= 96 && event.keyCode <= 105) ||
         event.keyCode === 188 ||
         event.keyCode === 8 ||
         event.keyCode === 9 ||
         event.keyCode === 17 ||
         event.keyCode === 46 ||
         event.keyCode === 37 ||
         event.keyCode === 38 ||
         event.keyCode === 39 ||
         event.keyCode === 40 ||
         event.keyCode === 35 ||
         event.keyCode === 36) {
        return true;
    }
    else {
        return false;
    }
}

function SetInputNumberNumberAndHurufOnly(event) {
    if ((event.keyCode >= 48 && event.keyCode <= 57) ||
        (event.keyCode >= 65 && event.keyCode <= 90) ||
        (event.keyCode >= 96 && event.keyCode <= 105) ||
         event.keyCode === 8 ||
         event.keyCode === 9 ||
         event.keyCode === 17 ||
         event.keyCode === 46 ||
         event.keyCode === 37 ||
         event.keyCode === 38 ||
         event.keyCode === 39 ||
         event.keyCode === 40 ||
         event.keyCode === 35 ||
         event.keyCode === 36) {
        return true;
    }
    else {
        return false;
    }
}

function GetNoBulanInd(bln)
{
    switch (bln.trim().toLowerCase()) {
        case "januari":
            return 1;            
        case "februari":
            return 2;
        case "maret":
            return 3;
        case "april":
            return 4;
        case "mei":
            return 5;
        case "juni":
            return 6;
        case "juli":
            return 7;
        case "agustus":
            return 8;
        case "september":
            return 9;
        case "oktober":
            return 10;
        case "november":
            return 11;
        case "desember":
            return 12;
    }
}

function GetNamaBulanInd(nomor) {
    switch (nomor) {
        case 1:
            return "Januari";
        case 2:
            return "Februari";
        case 3:
            return "Maret";
        case 4:
            return "April";
        case 5:
            return "Mei";
        case 6:
            return "Juni";
        case 7:
            return "Juli";
        case 8:
            return "Agustus";
        case 9:
            return "September";
        case 10:
            return "Oktober";
        case 11:
            return "November";
        case 12:
            return "Desember";
    }
}

function GetTanggalIndFromDateInd(date_ind, separator) {
    var arr_tanggal = date_ind.split(separator);
    if (arr_tanggal.length === 3) {
        var tanggal = arr_tanggal[0];
        var bulan = GetNamaBulanInd(parseInt(arr_tanggal[1]));
        var tahun = arr_tanggal[2];

        return tanggal + ' ' + bulan + ' ' + tahun;
    }
}

function GetTanggalWaktuIndToDateTime(_tgl)
{
    var arr_tgl = _tgl.split(" ");
    var valid_f = false;
    if (arr_tgl.length === 4) {
        var tgl = 0;
        var bln = 0;
        var thn = 0;

        var jam = 0;
        var mnt = 0;
        var dtk = 0;
        for (var i = 0; i < arr_tgl.length; i++) {
            switch (i) {
                case 0:
                    tgl = arr_tgl[i];
                    break;
                case 1:
                    bln = GetNoBulanInd(arr_tgl[i]) - 1;
                    break;
                case 2:
                    thn = arr_tgl[i];
                    break;
                case 3:
                    var stime = arr_tgl[i];
                    var arr_stime = stime.split(":");
                    if (arr_stime.length === 2)
                    {
                        valid_f = true;
                        for (var j = 0; j < arr_stime.length; j++) {
                            switch (j)
                            {
                                case 0:
                                    jam = arr_stime[j];
                                    break;
                                case 1:
                                    mnt = arr_stime[j];
                                    break;
                                case 2:
                                    dtk = arr_stime[j];
                                    break;
                            }
                        }
                    }
                    break;
            }
        }

        if (valid_f) {
            return new Date(
                    parseInt(thn), parseInt(bln), parseInt(tgl), parseInt(jam), parseInt(mnt), parseInt(dtk), 0                    
                );
        }
        else {
            return null;
        }
    }
    else {
        return null;
    }
}

function GetTanggalWaktuIndToDate(_tgl) {
    var arr_tgl = _tgl.split(" ");
    var valid_f = false;
    if (arr_tgl.length === 3) {
        var tgl = 0;
        var bln = 0;
        var thn = 0;
        for (var i = 0; i < arr_tgl.length; i++) {
            switch (i) {
                case 0:
                    tgl = arr_tgl[i];
                    break;
                case 1:
                    bln = GetNoBulanInd(arr_tgl[i]) - 1;
                    break;
                case 2:
                    thn = arr_tgl[i];
                    valid_f = true;
                    break;
            }
        }

        if (valid_f) {
            return new Date(
                    parseInt(thn), parseInt(bln), parseInt(tgl), 0, 0, 0, 0
                );
        }
        else {
            return null;
        }
    }
    else {
        return null;
    }
}

function GetDurasi(tgl1, tgl2)
{
    var dt1 = GetTanggalWaktuIndToDate(tgl1);
    var dt2 = GetTanggalWaktuIndToDate(tgl2);
    if (dt1 >= dt2) {
        return "";
    }
    if (dt1 !== null && dt2 !== null) {
        var dif = dt2 - dt1;
        var dif_s = dif / 1000;
        dif_s = Math.abs(dif_s);

        var sisa_bagi = dif_s;
        var hari = "";
        var jam = "";
        var menit = "";
        var detik = "";
        if (dif_s >= 86400)
        {
            sisa_bagi = dif_s % 86400;
            hari = (Math.floor(dif_s / 86400)).toString();            
        }
        if (parseFloat(sisa_bagi) >= 3600) {
            jam = (Math.floor(parseFloat(sisa_bagi) / 3600)).toString();
            sisa_bagi = parseFloat(sisa_bagi) % 3600;
        }
        if (parseFloat(sisa_bagi) >= 60) {
            menit = (Math.floor(parseFloat(sisa_bagi) / 60)).toString();
            sisa_bagi = parseFloat(sisa_bagi) % 60;
        }

        return (
                (hari.trim() !== "" ? hari + " hari " : "") +
                (jam.trim() !== "" ? jam + " jam " : "") +
                (menit.trim() !== "" ? menit + " menit " : "")
            ).trim();
    }
    else {
        return "";
    }
}

Object.defineProperty(Date.prototype, 'GetSerial', {
    value: function () {
        function pad2(n) {
            return (n < 10 ? '0' : '') + n;
        }

        return this.getFullYear() +
               pad2(this.getMonth() + 1) +
               pad2(this.getDate()) +
               pad2(this.getHours()) +
               pad2(this.getMinutes()) +
               pad2(this.getSeconds());
    }
});

function RenderDropDownOnTables() {
    // hold onto the drop down menu                                             
    var dropdownMenu;

    // and when you show it, move it to the body                                     
    $(window).on('show.bs.dropdown', function (e) {

        // grab the menu        
        dropdownMenu = $(e.target).find('.dropdown-menu-list-table');

        // detach it and append it to the body
        $('body').append(dropdownMenu.detach());

        // grab the new offset position
        var eOffset = $(e.target).offset();

        // make sure to place it where it would normally go (this could be improved)
        dropdownMenu.css({
            'display': 'block',
            'top': eOffset.top + $(e.target).outerHeight(),
            'left': eOffset.left
        });
    });

    // and when you hide it, reattach the drop down, and hide it normally                                                   
    $(window).on('hide.bs.dropdown', function (e) {
        $(e.target).append(dropdownMenu.detach());
        dropdownMenu.hide();
    });
}

var snackbarText = 1;

function GetPureNumber(replace, str) {
    var d = str.indexOf(",");
    var e = "";
    var desimal = "";
    var id0 = -1;
    if (d > -1) {
        e = str.substr(d);
        desimal = str.substr(d + 1);
        if (parseInt(desimal) === 0) {
            e = "";
        }
        if (desimal.length > 0) {
            for (var i = 0; i < desimal.length; i++) {
                if (desimal.substr(i, 1) === "0") {
                    id0 = i;
                    break;
                }
            }
            if (id0 > -1) {
                e = e.substr(0, id0 + 1);
            }
        }
        str = str.substring(0, d);
    }
    return str.replace(/[^0-9]+/g, replace) + e;
}

function SetTandaPemisahTitik(b) {
    if (b.trim() === "") {
        return "";
    }
    var _minus = false;
    if (b < 0) _minus = true;
    b = b.toString();
    b = b.replace(".", "");
    b = b.replace("-", "");
    var c = "";
    var d = b.indexOf(",");
    var e = "";
    if (d > -1) {
        e = b.substr(d);
        b = b.substring(0, d);
    }
    panjang = b.length;
    j = 0;
    for (i = panjang; i > 0; i--) {
        j = j + 1;
        if (((j % 3) === 1) && (j !== 1)) {
            c = b.substr(i - 1, 1) + "." + c;
        } else {
            c = b.substr(i - 1, 1) + c;
        }
    }
    if (_minus) c = "-" + c;
    return c + e;
}

function SetTandaPemisahTitikUseDesimal(b, jmldesimal) {
    if (b.indexOf(",") === b.length - 1) {
        b = b.replace(",", "");
    }
    if (b.trim() === "") {
        return "";
    }
    var _minus = false;
    if (b < 0) _minus = true;
    b = b.toString();
    b = b.replace(".", "");
    b = b.replace("-", "");
    var c = "";
    var d = b.indexOf(",");
    var e = "";
    var desimal = "";
    var i;
    for (i = 1; i <= jmldesimal; i++) {
        desimal += "0";
    }
    if (d > -1) {
        e = b.substr(d);
        b = b.substring(0, d);
        if (e.length - 1 < jmldesimal) {
            for (i = 1; i <= (jmldesimal - (e.length - 1)) ; i++) {
                e += "0";
            }
        }
        else if (e.length - 1 > jmldesimal) {
            e = e.substr(0, jmldesimal + 1);
        }
    }
    else {
        e = (desimal.trim() !== "" ? "," : "") + desimal;
    }
    panjang = b.length;
    j = 0;
    for (i = panjang; i > 0; i--) {
        j = j + 1;
        if (((j % 3) === 1) && (j !== 1)) {
            c = b.substr(i - 1, 1) + "." + c;
        } else {
            c = b.substr(i - 1, 1) + c;
        }
    }
    if (_minus) c = "-" + c;
    var hasil = c + e;
    if (jmldesimal === 0) {
        hasil = hasil.replace(",", "");
    }
    return hasil;
}

function isNumber(n) {
    n = n.replace(",", "");
    n = n.replace(".", "");
    return /^\d+(\.\d+)?$|^\.\d+$/.test(n);
}

var tableToExcel = (function () {
    var uri = 'data:application/vnd.ms-excel;base64,'
      , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" ' +
                   'xmlns:x="urn:schemas-microsoft-com:office:excel" ' +
                   'xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]>' +
                   '<xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name>' +
                   '<x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml>' +
                   '<![endif]--><meta http-equiv="content-type" content="text/plain; charset=UTF-8"/></head><body><table>{table}</table></body></html>'
      , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
      , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }
    return function (table, name) {
        if (!table.nodeType) table = document.getElementById(table)
        var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
        window.location.href = uri + base64(format(template, ctx))
    }
})();

function GetQueryString(name) {
    url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

Date.prototype.ddmmyyyy = function (separator) {
    var mm = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();

    return [(dd > 9 ? '' : '0') + dd,
            (mm > 9 ? '' : '0') + mm,
            this.getFullYear()
    ].join(separator);
};

Date.prototype.mmss = function (separator) {
    var mm = this.getHours();
    var ss = this.getMinutes();

    return [(mm > 9 ? '' : '0') + mm,
            (ss > 9 ? '' : '0') + ss
    ].join(separator);
};

Date.prototype.yyyymmdd = function (separator) {
    var mm = this.getMonth() + 1; // getMonth() is zero-based
    var dd = this.getDate();

    return [this.getFullYear(),
            (mm > 9 ? '' : '0') + mm,
            (dd > 9 ? '' : '0') + dd            
    ].join(separator);
};

function DeleteTableRow(rowid) {
    var row = document.getElementById(rowid);
    var table = row.parentNode;
    while (table && table.tagName !== 'TABLE')
        table = table.parentNode;
    if (!table)
        return;
    table.deleteRow(row.rowIndex);
}

function GetTableRowCount(tableid) {
    var table = document.getElementById(tableid);
    if (table !== undefined && table !== null) {
        return table.rows.length;
    }
    return 0;
}