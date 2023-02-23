//general script
//-----------------------------------
function DoListBiayaToHiddenField(hfl_target, txt_biaya_name) {
	var arr_biaya = document.getElementsByName(txt_biaya_name);
	if (hfl_target !== null && hfl_target !== undefined) {
		hfl_target.value = "";
		if (arr_biaya.length > 0) {
			for (var i = 0; i < arr_biaya.length; i++) {
				hfl_target.value += arr_biaya[i].lang + "|" + arr_biaya[i].value + ";";
			}
		}
	}
}

function ShowConfirmSavePengaturan() {
	HideMsgConfirm();

	$('#ui_modal_confirm_save_pengaturan').modal({
		backdrop: 'static',
		keyboard: false
	}, 'show');
}

function DoInitPicker(id_picker) {
	$('#' + id_picker).pickdate({
		cancel: 'Hapus Tanggal',
		closeOnCancel: false,
		closeOnSelect: true,
		container: '',
		firstDay: 1,
		format: 'dd mmmm yyyy',
		formatSubmit: 'dd mmmm yyyy',
		ok: 'Tutup',
		selectMonths: true,
		selectYears: 6,
		today: ''
	});
}

function CheckValidationGroup(valGrp) {
	var message = "";
	var rtnVal = true;
	for (i = 0; i < Page_Validators.length; i++) {
		if (Page_Validators[i].validationGroup === valGrp) {
			ValidatorValidate(Page_Validators[i]);
			if (!Page_Validators[i].isvalid) {
				rtnVal = false;
				break;

			}
		}
	}
	return rtnVal;
}

//pengaturan kelas aktif
//-----------------------------------
function ParseKelasAktif(hfl_target) {
    var arr_kelas = document.getElementsByName("CHK_KELAS[]");
    var arr_kelas_mapel = document.getElementsByName("TXT_KELAS_MAPEL[]");
	if (hfl_target !== null && hfl_target !== undefined) {
		hfl_target.value = "";
		if (arr_kelas.length > 0 && arr_kelas_mapel.length === arr_kelas.length) {
			for (var i = 0; i < arr_kelas.length; i++) {
				if (arr_kelas[i].checked) {
				    hfl_target.value += arr_kelas[i].value +
                                        "|" +
				                        arr_kelas_mapel[i].value +
				                        ";";
				}
			}
		}
	}    
}

//pengaturan batasan usia
//-----------------------------------
function ParseBatasanUsia(hfl_target) {
	var arr_tahun_max = document.getElementsByName("CBO_TAHUN_MAX[]");
	var arr_bulan_max = document.getElementsByName("CBO_BULAN_MAX[]");

	var arr_tahun_min = document.getElementsByName("CBO_TAHUN_MIN[]");
	var arr_bulan_min = document.getElementsByName("CBO_BULAN_MIN[]");

	var arr_kelas = document.getElementsByName("HFL_KELAS_BATAS_USIA[]");

	if (hfl_target !== null && hfl_target !== undefined) {
		hfl_target.value = "";
		if (
			arr_tahun_max.length > 0 &&
			arr_tahun_max.length === arr_bulan_max.length &&
			arr_tahun_max.length === arr_tahun_min.length &&
			arr_tahun_max.length === arr_bulan_min.length &&
			arr_tahun_max.length === arr_kelas.length 
		) {
			for (var i = 0; i < arr_tahun_max.length; i++) {
				hfl_target.value += arr_kelas[i].value + "|" +
									arr_tahun_min[i].value + "|" + arr_bulan_min[i].value + "|" +
									arr_tahun_max[i].value + "|" + arr_bulan_max[i].value + ";";
			}
		}
	}
}

//pengaturan jadwal wawancara
//-----------------------------------
var tbody_wawancara = function () { return document.getElementById("tbody_wawancara"); }

function ShowJadwalKosong() {
	if (tbody_wawancara() !== undefined && tbody_wawancara() !== null) {
		tbody_wawancara().innerHTML = "<tr>" +
										"<td colspan=\"5\" style=\"text-align: center; color: grey; font-weight: bold;\">" +
											"..:: Data Kosong ::.." +
										"</td>" +
									"</tr>"
	}
}

function AddJadwalWawancara(s_dari_tanggal, s_sampai_tanggal, dari_jam, sampai_jam, kuota, waktu_sesi_menit) {
	if (tbody_wawancara() !== undefined && tbody_wawancara() !== null) {
		var dari_tanggal = GetTanggalWaktuIndToDate(s_dari_tanggal);
		var sampai_tanggal = GetTanggalWaktuIndToDate(s_sampai_tanggal);

		if (dari_tanggal <= sampai_tanggal) {

			var id = 1;
			var kode = new Date().GetSerial();
			var arr_dari_jam = dari_jam.split(':');
			var arr_sampai_jam = sampai_jam.split(':');
			
			for (var tanggal = dari_tanggal; tanggal <= sampai_tanggal; tanggal.setDate(tanggal.getDate() + 1)) {
				var tanggal_jam = new Date(tanggal.getFullYear(),
										   tanggal.getMonth(),
										   tanggal.getDay(),
										   parseInt(arr_dari_jam[0]),
										   parseInt(arr_dari_jam[1])
										);
				var tanggal_jam_selesai = new Date(tanggal.getFullYear(),
												   tanggal.getMonth(),
												   tanggal.getDay(),
												   parseInt(arr_sampai_jam[0]),
												   parseInt(arr_sampai_jam[1])
												);

				var id2 = 1;
				for (var tgl_jam = tanggal_jam;
						 tgl_jam < tanggal_jam_selesai;
						 tgl_jam.setMinutes(tgl_jam.getMinutes() + parseInt(waktu_sesi_menit))) {

					var tr_kode = "TR_" + kode + (1000 + (id * 100) + id2).toString();
					var chk_kode = "CHK_" + kode + (1000 + (id * 100) + id2).toString();

					var jam_wawancara = tgl_jam;
					var jam_wawancara_mulai = jam_wawancara.mmss(':');
					jam_wawancara.setMinutes(jam_wawancara.getMinutes() + parseInt(waktu_sesi_menit));
					var jam_wawancara_selesai = jam_wawancara.mmss(':');

					if (jam_wawancara > tanggal_jam_selesai) break;

					var jam_wawancara_pilih = jam_wawancara_mulai + " - " + jam_wawancara_selesai;

					tbody_wawancara().innerHTML += "<tr tr_jadwal[] id=\"" + tr_kode + "\">" +
														"<td style=\"width: 190px; " +
																	"text-align: left; " +
																	"background-color: #fafafa; " +
																	"border-bottom-style: dashed; " +
																	"border-bottom-color: #d3d3d3; " +
																	"border-bottom-width: 1px; " +
																	"vertical-align: middle; " +
														"\">" +
															"<div class=\"checkbox checkbox-adv\">" +
																"<label class=\"pull-left\" for=\"" + chk_kode + "\" style=\"font-weight: bold;\">" +
																	"<input value=\"" + tr_kode + "\" " +
																			"class=\"access-hide\" " +
																			"id=\"" + chk_kode + "\" " +
																			"name=\"CHK_TGL_WAWANCARA[]\" " +
																			"type=\"checkbox\">" +
																	tanggal.ddmmyyyy('/') +
																	"<span class=\"checkbox-circle\"></span>" +
																	"<span class=\"checkbox-circle-check\"></span>" +
																	"<span class=\"checkbox-circle-icon icon\">done</span>" +
																"</label>" +
																"<input type=\"hidden\" " +
																	   "name=\"HFL_WAWANCARA[]\" " +
																	   "value=\"" + "" + "|" +
                                                                                    tanggal.yyyymmdd('-') + "|" +
																					jam_wawancara_pilih + "|" +
																					waktu_sesi_menit +
																			 "\" " +
																"/>" +
															"</div>" +
														"</td>" +
														"<td style=\"text-align: center; " +
																	 "background-color: #fafafa; " +
																	 "border-bottom-style: dashed; " +
																	 "border-bottom-color: #d3d3d3; " +
																	 "border-bottom-width: 1px; " +
																	 "vertical-align: middle;\" " +
														">" +
															"<label " +
																"onclick=\"ShowEditJadwal('" + tr_kode + "', '" +
																							   tanggal.ddmmyyyy('/') + "', '" +
																							   jam_wawancara_mulai + "', '" +
																							   jam_wawancara_selesai + "', " +
                                                                                               "GetKuota('" + tr_kode + "'), " +
                                                                                               waktu_sesi_menit
																					     ");\" " +
																"class=\"pull-right\" " +
																"title=\" Edit \" " +
																"style=\"padding-left: 0px; color: #1DA1F2; cursor: pointer;\">" +
																	"<i class=\"fa fa-pencil\"></i>" +
															"</label>" +
														"</td>" +
														"<td style=\"text-align: center; " +
																	"background-color: #fafafa; " +
																	"border-bottom-style: dashed; " +
																	"border-bottom-color: #d3d3d3; " +
																	"border-bottom-width: 1px; " +
																	"vertical-align: middle;\">" +
															jam_wawancara_pilih +
														"</td>" +
														"<td style=\"text-align: center; " +
																	"background-color: #fafafa; " +
																	"border-bottom-style: dashed; " +
																	"border-bottom-color: #d3d3d3; " +
																	"border-bottom-width: 1px; " +
																	"vertical-align: middle;\">" +
															kuota +
														"</td>" +
														"<td style=\"text-align: center; " +
																	"background-color: #fafafa; " +
																	"border-bottom-style: dashed; " +
																	"border-bottom-color: #d3d3d3; " +
																	"border-bottom-width: 1px; " +
																	"vertical-align: middle;\">" +
															waktu_sesi_menit +
														"</td>" +
													"</tr>";

					tgl_jam.setMinutes(jam_wawancara.getMinutes() - parseInt(waktu_sesi_menit));
					id2++;
				}

				id++;
			}

			return true;
		}
	}

	return false;
}

function GetKuota(id_tr) {
	if (tbody_wawancara() !== undefined && tbody_wawancara() !== null) {
		var table = document.getElementById('tb_wawancara');
		for (var i = 0; i < table.rows.length; i++) {
			var trs = table.getElementsByTagName("tr")[i];
			if (trs.id === id_tr) {
				var cellVal = trs.cells[3];
				return cellVal.innerHTML;
			}
		}
	}
	return "";
}

function UpdateJadwalWawancara(id_tr, kuota) {
	if (tbody_wawancara() !== undefined && tbody_wawancara() !== null) {
		var table = document.getElementById('tb_wawancara');
		for (var i = 0; i < table.rows.length; i++) {
			var trs = table.getElementsByTagName("tr")[i];
			if (trs.id === id_tr) {
				var cellVal = trs.cells[3];
				cellVal.innerHTML = kuota;
				return true;
			}
		}
	}
	return false;
}

function ParseJadwalWawancara(hfl_target) {
	if (hfl_target !== null && hfl_target !== undefined) {
		hfl_target.value = "";
		if (tbody_wawancara() !== undefined && tbody_wawancara() !== null) {
			var table = document.getElementById('tb_wawancara');
			if (table.rows.length > 1) {
				var arr_hfl_wawancara = document.getElementsByName("HFL_WAWANCARA[]");
				if (arr_hfl_wawancara.length === (table.rows.length - 1)) {
					for (var i = 1; i < table.rows.length; i++) {
						var trs = table.getElementsByTagName("tr")[i];
						var cellVal = trs.cells;
						if (cellVal.length === 5) {
							hfl_target.value += arr_hfl_wawancara[i - 1].value + "|" +
												cellVal[3].innerHTML + ";";
						}
					}
				}
			}
		}
	}	
}

function ShowEditJadwal(id_tr, tanggal, dari_jam, sampai_jam, kuota, lama_per_sesi) {
	var txt_tgl = document.getElementById("txtTanggalWawancaraEdit");
	var txt_jam_dari = document.getElementById("txtWawancaraDariSampaiEdit");
	var txt_jam_sampai = document.getElementById("txtWawancaraDariJamEdit");
	var txt_kuota = document.getElementById("txtKuotaWawancaraEdit");
	var txt_lama_per_sesi = document.getElementById("txtLamaPerSesiEdit");

	if (
		txt_tgl !== undefined && txt_tgl !== undefined &&
		txt_jam_dari !== undefined && txt_jam_dari !== undefined &&
		txt_jam_sampai !== undefined && txt_jam_sampai !== undefined &&
		txt_kuota !== undefined && txt_kuota !== undefined &&
        txt_lama_per_sesi !== undefined && txt_lama_per_sesi !== undefined
	) {
		txt_tgl.lang = id_tr;
		txt_tgl.value = GetTanggalIndFromDateInd(tanggal, '/');
		txt_jam_dari.value = dari_jam;
		txt_jam_sampai.value = sampai_jam;
		txt_kuota.value = kuota;
		txt_lama_per_sesi.value = lama_per_sesi;

		$('#ui_modal_edit_jadwal_wawancara').modal({
			backdrop: 'static',
			keyboard: false
		}, 'show');
	}
}

function DeleteSelectedJadwalRows() {
	var arr_chk = document.getElementsByName("CHK_TGL_WAWANCARA[]");
	if (arr_chk.length > 0) {
		for (var i = arr_chk.length - 1; i >= 0; i--) {
			if (arr_chk[i].checked) {
				DeleteTableRow(arr_chk[i].value);
			}
		}

		if (GetTableRowCount('tb_wawancara') === 1) {
			ShowJadwalKosong();
		}
	}
}

function ListJamToDropdown(cbo) {
	if (cbo !== undefined && cbo !== null) {
		var length = cbo.options.length;
		for (i = length - 1; i >= 0; i--) {
			cbo.options[i] = null;
		}

		var option = document.createElement('option');
		option.text = "";
		option.value = "";
		cbo.appendChild(option);

		var arr_menit = ["00", "15", "30", "45"];
		for (var i = 0; i <= 23; i++) {
			for (var m = 0; m < arr_menit.length; m++) {
				option = document.createElement('option');
				option.text = (i < 10 ? "0" : "") + i.toString() + ":" + arr_menit[m];
				option.value = option.text;
				cbo.appendChild(option);
			}            
		}
	}
}