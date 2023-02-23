<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.Siswa.Input.aspx.cs" Inherits="AI_ERP.Application_Modules.MASTER.wf_Siswa_Input" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .input-box {
            border-style: solid;
            border-width: 1px;
            border-color: #DBDBDB;
            padding: 6px;
            outline : none;
            width: 100%;
            margin-top: 5px;
            font-size: small;
            font-weight: bold;
            color: grey;
        }
    </style>
    <script type="text/javascript">
        function InitPicker() {
            $('#<%= txtTanggalLahir.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 40, today: '' });
            $('#<%= txtTanggalLahirAyah.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 160, today: '' });
            $('#<%= txtTanggalLahirIbu.ClientID %>').pickdate({ cancel: '', closeOnCancel: false, closeOnSelect: true, container: '', firstDay: 1, format: 'dd mmmm yyyy', formatSubmit: 'dd mmmm yyyy', ok: 'Tutup', selectMonths: true, selectYears: 160, today: '' });
        }

        function EndRequestHandler() {
			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }

        function EndRequest() {
            var jenis_act = document.getElementById("<%= txtKeyAction.ClientID %>").value;
            switch (jenis_act) {
                case "<%= JenisAction.DoUpdate %>":
                    $('body').snackbar({
                        alive: 2000,
                        content: '<i class=\"fa fa-info-circle\"></i>&nbsp;&nbsp;&nbsp;Data sudah disimpan',
                        show: function () {
                            snackbarText++;
                        }
                    });
                    setTimeout(function () { ShowProgress(false); }, 500);
                    break;
                default:
                    if (jenis_act.trim() != ""){
                        $('body').snackbar({
                            alive: 6000,
                            content: '<i class=\"fa fa-exclamation-triangle\"></i>&nbsp;&nbsp;&nbsp;PERHATIAN : ' + jenis_act,
                            show: function () {
                                snackbarText++;
                            }
                        });
                    }
                    break;
            }
            document.getElementById("<%= txtKeyAction.ClientID %>").value = "";
            InitPicker();
        }

        function ShowProgress(value) {
            if (value) {
                pb_top.style.display = "";
            } else {
                pb_top.style.display = "none";
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="pb_top" class="progress progress-position-absolute-top" style="display: none; position: fixed; top: 0px; right: 0px; z-index: 9999999;">
		<div class="load-bar">
			<div class="load-bar-base">
				<div class="load-bar-content">
					<div class="load-bar-progress"></div>
					<div class="load-bar-progress load-bar-progress-brand"></div>
					<div class="load-bar-progress load-bar-progress-green"></div>
					<div class="load-bar-progress load-bar-progress-orange"></div>
				</div>
			</div>
		</div>
		<div class="load-bar">
			<div class="load-bar-base">
				<div class="load-bar-content">
					<div class="load-bar-progress"></div>
					<div class="load-bar-progress load-bar-progress-orange"></div>
					<div class="load-bar-progress load-bar-progress-green"></div>
					<div class="load-bar-progress load-bar-progress-brand"></div>
				</div>
			</div>
		</div>
	</div>

    <asp:UpdatePanel runat="server" ID="upMain">
        <ContentTemplate>

            <asp:HiddenField runat="server" ID="txtID" />

            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowDetail" OnClick="btnShowDetail_Click" style="position: absolute; left: -1000px; top: -1000px;" />
            <asp:Button UseSubmitBehavior="false" runat="server" ID="btnShowConfirmDelete" OnClick="btnShowConfirmDelete_Click" style="position: absolute; left: -1000px; top: -1000px;" />

            <div class="row" style="margin-left: 0px; margin-right: 0px;">
                <div class="col-lg-8 col-lg-offset-2 col-md-8 col-md-offset-2">
		            <section class="content-inner margin-top-no">
			            <div class="card" style="box-shadow: 0 0px 0 #e5e5e5, 0 0 2px rgba(0, 0, 0, 0.12), 0 2px 4px rgba(0, 0, 0, 0.24); box-shadow: none; border-top-left-radius: unset; border-top-right-radius: unset; margin-top: 0px;">
				            <div class="card-main">

                                <asp:HiddenField runat="server" ID="txtKeyAction" />
                                <asp:HiddenField runat="server" ID="txtIDPSB" />
            
                                <div class="row" style="margin-left: 25px; margin-right: 25px;">
                                    <div id="content_div_isi_biodata_calon_siswa" class="tile" 
                                        style="box-shadow: none; margin-bottom: 10px; margin-top: 25px;">
				                        <div style="margin: 0 auto; display: table; height: 100px; width: 100px; border-radius: 100%; background: transparent;">
                                            <asp:Literal runat="server" ID="ltrFotoSiswa"></asp:Literal>
					                    </div>
			                        </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Data Pribadi Siswa
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNISSekolah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIS Sekolah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNISSekolah"></asp:TextBox> 
					                        </div>
                                        </div>
				                        <div class="col-md-8">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNama.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap (sesuai akte kelahiran)
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNama"></asp:TextBox> 
                                                <asp:RequiredFieldValidator ValidationGroup="vldInput" runat="server" ID="vldNamaLengkap"
                                                    ControlToValidate="txtNama" Display="Dynamic" style="float: right; font-weight: bold;"
                                                    Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtPanggilan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Panggilan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtPanggilan"></asp:TextBox> 
					                        </div>
				                        </div>
			                            <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboJenisKelamin.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Laki-Laki" Value="L"></asp:ListItem>
                                                    <asp:ListItem Text="Perempuan" Value="P"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTahunPelajaran.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tahun Pelajaran
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTahunPelajaran"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
                                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboUnitSekolah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Unit Sekolah
						                        </label>
						                        <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="cboUnitSekolah_SelectedIndexChanged" ValidationGroup="vldInput" runat="server" ID="cboUnitSekolah" CssClass="input-box"></asp:DropDownList>
					                        </div>
				                        </div>
                                    </div>
                                    <div class="row">
			                            <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboKelasRombel.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <asp:Literal runat="server" ID="ltrJudulKelasRombel"></asp:Literal>                                                    
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboKelasRombel" CssClass="input-box"></asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-4" runat="server" id="div_kelaspeminatan">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboKelasJurusan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kelas Peminatan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboKelasJurusan" CssClass="input-box"></asp:DropDownList>
					                        </div>
				                        </div>                                        
                                        <div class="col-md-4" runat="server" id="div_kelassosialisasi">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboKelasSosialisasi.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kelas Sosialisasi
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboKelasSosialisasi" CssClass="input-box"></asp:DropDownList>
					                        </div>
				                        </div>                                        
			                        </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtEmail.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Email
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtEmail"></asp:TextBox> 
					                        </div>
				                        </div>
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTempatLahir.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTempatLahir"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTanggalLahir.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTanggalLahir"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboDiterimaDiKelas.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Diterima di Level
						                        </label>
                                                <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboDiterimaDiKelas" CssClass="input-box"></asp:DropDownList>
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-2">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboAgama.ClientID %>" style="color: #B7770D; font-size: small;">Agama</label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboAgama" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Islam" Value="Islam"></asp:ListItem>
                                                    <asp:ListItem Text="Protestan" Value="Protestan"></asp:ListItem>
                                                    <asp:ListItem Text="Katolik" Value="Katolik"></asp:ListItem>
                                                    <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                                    <asp:ListItem Text="Budha" Value="Budha"></asp:ListItem>
                                                    <asp:ListItem Text="Konghuchu" Value="Konghuchu"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-2">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboStatusAnak.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Status Anak
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboStatusAnak" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Kandung" Value="Kandung"></asp:ListItem>
                                                    <asp:ListItem Text="Tiri" Value="Tiri"></asp:ListItem>
                                                    <asp:ListItem Text="Yatim" Value="Yatim"></asp:ListItem>
                                                    <asp:ListItem Text="Piatu" Value="Piatu"></asp:ListItem>
                                                    <asp:ListItem Text="Yatim Piatu" Value="Yatim Piatu"></asp:ListItem>
                                                    <asp:ListItem Text="Adopsi" Value="Adopsi"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
			                            <div class="col-md-2">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNama.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Anak Ke
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAnakKe"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-2">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtJumlahSaudara.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Dari bersaudara
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtJumlahSaudara"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-2">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtJumlahKakak.ClientID %>" style="color: #B7770D; font-size: small;">Jumlah Kakak</label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtJumlahKakak"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-2">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtJumlahAdik.ClientID %>" style="color: #B7770D; font-size: small;">Jumlah Adik</label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtJumlahAdik"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboAsalSekolah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Asal Sekolah
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboAsalSekolah" CssClass="input-box">                                                            
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-8">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTempatLahir.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Asal Sekolah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNamaAsalSekolah"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNISLama.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIS Al-Izhar
						                        </label>
						                        <asp:TextBox Enabled="false" ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNISLama"></asp:TextBox> 
					                        </div>
                                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNISN.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NISN
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNISN"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNIK.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK (sesuai kartu keluarga)
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNIK"></asp:TextBox> 
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtWargaNegara.ClientID %>" style="color: #B7770D; font-size: small;">Warga Negara</label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtWargaNegara"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-8">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtBahasaSehariHari.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Bahasa Sehari-hari
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtBahasaSehariHari"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboHobi.ClientID %>" style="color: #B7770D; font-size: small;">Hobi</label>
						                        <asp:DropDownList ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="cboHobi">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Olahraga" Value="Olahraga"></asp:ListItem>
                                                    <asp:ListItem Text="Kesenian" Value="Kesenian"></asp:ListItem>
                                                    <asp:ListItem Text="Membaca" Value="Membaca"></asp:ListItem>
                                                    <asp:ListItem Text="Traveling" Value="Traveling"></asp:ListItem>
                                                    <asp:ListItem Text="Menulis" Value="Menulis"></asp:ListItem>
                                                    <asp:ListItem Text="Lainnya" Value="Lainnya"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-8">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtHobiLainnya.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hobi Lainnya
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtHobiLainnya"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Data Tempat Tinggal Siswa
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtAlamat.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Alamat
						                        </label>
						                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAlamat" style="resize: none;"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
				                        <div class="col-md-1">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtRT.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    RT
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtRT"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-1">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtRW.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    RW
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtRW"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtKelurahan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kelurahan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKelurahan"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-6">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtKecamatan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kecamatan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKecamatan"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
                                        <div class="col-md-6">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtKabupatenKota.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kabupaten/Kota
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKabupatenKota"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-6">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtProvinsi.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Provinsi
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtProvinsi"></asp:TextBox> 
					                        </div>
				                        </div>                    
			                        </div>

                                    <div class="row">
                                        <div class="col-md-2">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtKodePOS.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kode POS
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKodePOS"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboStatusTempatTinggal.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Status Tempat Tinggal
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboStatusTempatTinggal" CssClass="input-box">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="Tempat Tinggal Orang Tua" Text="Tempat Tinggal Orang Tua"></asp:ListItem>
                                                    <asp:ListItem Value="Menumpang Orang Lain" Text="Menumpang Orang Lain"></asp:ListItem>
                                                    <asp:ListItem Value="Asrama" Text="Asrama"></asp:ListItem>
                                                    <asp:ListItem Value="Lainnya" Text="Lainnya"></asp:ListItem>                              
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-3">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboJarakKeSekolah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jarak ke Sekolah
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJarakKeSekolah" CssClass="input-box">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="0-1 KM" Text="0-1 KM"></asp:ListItem>
                                                    <asp:ListItem Value="1-3 KM" Text="1-3 KM"></asp:ListItem>
                                                    <asp:ListItem Value="3-5 KM" Text="3-5 KM"></asp:ListItem>
                                                    <asp:ListItem Value="5-10 KM" Text="5-10 KM"></asp:ListItem>
                                                    <asp:ListItem Value=">10 KM" Text=">10 KM"></asp:ListItem>                                
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-3">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboKeSekolahDengan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Ke Sekolah dengan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboKeSekolahDengan" CssClass="input-box">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="Kendaraan Umum" Text="Kendaraan Umum"></asp:ListItem>
                                                    <asp:ListItem Value="Kendaraan Pribadi" Text="Kendaraan Pribadi"></asp:ListItem>
                                                    <asp:ListItem Value="Jalan Kaki" Text="Jalan Kaki"></asp:ListItem>                        
						                        </asp:DropDownList>
					                        </div>
				                        </div>                    
			                        </div>

                                    <div class="row">
                                        <div class="col-md-6">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtWaktuTempuh.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Waktu Tempuh (jam:menit)
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtWaktuTempuh"></asp:TextBox> 
					                        </div>
				                        </div> 
                                    </div>

                                    <div runat="server" id="div_riwayat_pendidikan">

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                            <i class="fa fa-hashtag"></i>
						                            Riwayat Pendidikan Siswa
					                            </div>
                                            </div>
                                        </div>

                                        <div class="row" runat="server" id="div_asal_tk">
				                            <div class="col-md-12">
					                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                            <label for="<%= txtTK.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        TK
						                            </label>
						                            <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTK"></asp:TextBox> 
					                            </div>
				                            </div>
                                        </div>

                                        <div class="row" runat="server" id="div_asal_sd">
				                            <div class="col-md-12">
					                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                            <label for="<%= txtSD.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        SD
						                            </label>
						                            <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtSD"></asp:TextBox> 
					                            </div>
				                            </div>
                                        </div>

                                        <div class="row" runat="server" id="div_asal_smp">
				                            <div class="col-md-12">
					                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                            <label for="<%= txtSMP.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        SMP
						                            </label>
						                            <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtSMP"></asp:TextBox> 
					                            </div>
				                            </div>
                                        </div>

                                        <div class="row" runat="server" id="div_asal_sma">
				                            <div class="col-md-12">
					                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                            <label for="<%= txtSMA.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        SMA
						                            </label>
						                            <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtSMA"></asp:TextBox> 
					                            </div>
				                            </div>
                                        </div>

                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Bakat Khusus dan Prestasi Siswa
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtKesenian.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kesenian
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKesenian"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtOlahraga.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Olahraga (Jasmani)
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtOlahraga"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtKemasyarakatan.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Kemasyarakatan/Organisasi
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtKemasyarakatan"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtBakatLain.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Lain-lain
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtBakatLain"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Data Orang Tua Siswa
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboStatusHubOrtu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Status hubungan dengan orang tua
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboStatusHubOrtu" CssClass="input-box">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="Kandung" Text="Kandung"></asp:ListItem>
                                                    <asp:ListItem Value="Wali" Text="Wali"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>                    
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboStatusPernikahanOrtu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Status pernikahan orang tua
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboStatusPernikahanOrtu" CssClass="input-box">
                                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                                    <asp:ListItem Value="Utuh" Text="Utuh"></asp:ListItem>
                                                    <asp:ListItem Value="Cerai" Text="Cerai"></asp:ListItem>
                                                    <asp:ListItem Value="Berpisah" Text="Berpisah"></asp:ListItem>
                                                    <asp:ListItem Value="Lainnya" Text="Lainnya"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>                    
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label style="color: #B7770D; font-size: small;">
                                                    Siswa tinggal bersama
						                        </label>
					                        </div>
				                        </div>                    
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkAyahKandung.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkAyahKandung.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkAyahKandung" type="checkbox" />
										                    <span class="switch-toggle"></span>Ayah Kandung
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkIbuKandung.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkIbuKandung.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkIbuKandung" type="checkbox" />
										                    <span class="switch-toggle"></span>Ibu Kandung
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkAyahTiri.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkAyahTiri.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkAyahTiri" type="checkbox" />
										                    <span class="switch-toggle"></span>Ayah Tiri
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkIbuTiri.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkIbuTiri.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkIbuTiri" type="checkbox" />
										                    <span class="switch-toggle"></span>Ibu Tiri
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkKakek.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkKakek.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkKakek" type="checkbox" />
										                    <span class="switch-toggle"></span>Kakek
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkNenek.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkNenek.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkNenek" type="checkbox" />
										                    <span class="switch-toggle"></span>Nenek
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>                    
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkKakak.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkKakak.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkKakak" type="checkbox" />
										                    <span class="switch-toggle"></span>Kakak
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                        <label for="<%= chkAdik.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    <div class="checkbox switch" style="padding-bottom: 0px;">
									                    <label for="<%= chkAdik.ClientID %>" style="color: grey; font-weight: bold;">
										                    <input runat="server" class="access-hide" id="chkAdik" type="checkbox" />
										                    <span class="switch-toggle"></span>Adik
									                    </label>
								                    </div>
						                        </label>
					                        </div>
                                        </div>                    
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 5px;">
						                        <label for="<%= txtTinggalDenganLainnya.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Lain-lain
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTinggalDenganLainnya"></asp:TextBox> 
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Data Ayah
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNamaAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNamaAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTempatLahirAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTempatLahirAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTanggalLahirAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTanggalLahirAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboAgamaAyah.ClientID %>" style="color: #B7770D; font-size: small;">Agama Ayah</label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboAgamaAyah" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Islam" Value="Islam"></asp:ListItem>
                                                    <asp:ListItem Text="Protestan" Value="Protestan"></asp:ListItem>
                                                    <asp:ListItem Text="Katolik" Value="Katolik"></asp:ListItem>
                                                    <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                                    <asp:ListItem Text="Budha" Value="Budha"></asp:ListItem>
                                                    <asp:ListItem Text="Konghuchu" Value="Konghuchu"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTempatLahirAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Suku Bangsa Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtSukuBangsaAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtWargaNegaraAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Warga Negara Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtWargaNegaraAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboPendidikanAyah.ClientID %>" style="color: #B7770D; font-size: small;">Pendidikan Ayah</label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboPendidikanAyah" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                                    <asp:ListItem Text="SMP" Value="SMP"></asp:ListItem>
                                                    <asp:ListItem Text="SMA" Value="SMA"></asp:ListItem>
                                                    <asp:ListItem Text="D1" Value="D1"></asp:ListItem>
                                                    <asp:ListItem Text="D2" Value="D2"></asp:ListItem>
                                                    <asp:ListItem Text="D3" Value="D3"></asp:ListItem>
                                                    <asp:ListItem Text="D4" Value="D4"></asp:ListItem>
                                                    <asp:ListItem Text="S1" Value="S1"></asp:ListItem>
                                                    <asp:ListItem Text="S2" Value="S2"></asp:ListItem>
                                                    <asp:ListItem Text="S3" Value="S3"></asp:ListItem>
                                                    <asp:ListItem Text="Lainnya" Value="Lainnya"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtPendidikanAyahLainnya.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Isi jika pilih pendidikan lainnya
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtPendidikanAyahLainnya"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtJurusanPendidikanAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jurusan Pendidikan Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtJurusanPendidikanAyah"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtAlamatAyah.ClientID %>" style="color: #B7770D; font-size: small;">Alamat Rumah Ayah</label>
						                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAlamatAyah" style="resize: none;"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNIKAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNIKAyah"></asp:TextBox> 
					                        </div>
				                        </div>
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNoTelponAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.Telpon/HP Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNoTelponAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtEmailAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Email Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtEmailAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtPekerjaanAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Pekerjaan Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtPekerjaanAyah"></asp:TextBox> 
					                        </div>
				                        </div>
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNamaInstansiAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Instansi Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNamaInstansiAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNoTelponKantorAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.Telp. Kantor Ayah
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNoTelponKantorAyah"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtAlamatKantorAyah.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Alamat Kantor Ayah
						                        </label>
						                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAlamatKantorAyah" style="resize: none;"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>
                
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Data Ibu
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNamaIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNamaIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTempatLahirIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tempat Lahir Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTempatLahirIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTanggalLahirIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Tanggal Lahir Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtTanggalLahirIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboAgamaIbu.ClientID %>" style="color: #B7770D; font-size: small;">Agama Ibu</label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboAgamaIbu" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Islam" Value="Islam"></asp:ListItem>
                                                    <asp:ListItem Text="Protestan" Value="Protestan"></asp:ListItem>
                                                    <asp:ListItem Text="Katolik" Value="Katolik"></asp:ListItem>
                                                    <asp:ListItem Text="Hindu" Value="Hindu"></asp:ListItem>
                                                    <asp:ListItem Text="Budha" Value="Budha"></asp:ListItem>
                                                    <asp:ListItem Text="Konghuchu" Value="Konghuchu"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtTempatLahirIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Suku Bangsa Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtSukuBangsaIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtWargaNegaraIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Warga Negara Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtWargaNegaraIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= cboPendidikanIbu.ClientID %>" style="color: #B7770D; font-size: small;">Pendidikan Ibu</label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboPendidikanIbu" CssClass="input-box">
                                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                                    <asp:ListItem Text="SMP" Value="SMP"></asp:ListItem>
                                                    <asp:ListItem Text="SMA" Value="SMA"></asp:ListItem>
                                                    <asp:ListItem Text="D1" Value="D1"></asp:ListItem>
                                                    <asp:ListItem Text="D2" Value="D2"></asp:ListItem>
                                                    <asp:ListItem Text="D3" Value="D3"></asp:ListItem>
                                                    <asp:ListItem Text="D4" Value="D4"></asp:ListItem>
                                                    <asp:ListItem Text="S1" Value="S1"></asp:ListItem>
                                                    <asp:ListItem Text="S2" Value="S2"></asp:ListItem>
                                                    <asp:ListItem Text="S3" Value="S3"></asp:ListItem>
                                                    <asp:ListItem Text="Lainnya" Value="Lainnya"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtPendidikanIbuLainnya.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Isi jika pilih pendidikan lainnya
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtPendidikanIbuLainnya"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtJurusanPendidikanIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jurusan Pendidikan Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtJurusanPendidikanIbu"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtAlamatIbu.ClientID %>" style="color: #B7770D; font-size: small;">Alamat Rumah Ibu</label>
						                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAlamatIbu" style="resize: none;"></asp:TextBox> 
					                        </div>
				                        </div>
			                        </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNIKIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    NIK Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNIKIbu"></asp:TextBox> 
					                        </div>
				                        </div>
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNoTelponIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.Telpon/HP Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNoTelponIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtEmailIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Email Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtEmailIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtPekerjaanIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Pekerjaan Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtPekerjaanIbu"></asp:TextBox> 
					                        </div>
				                        </div>
				                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNamaInstansiIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Instansi Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNamaInstansiIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-4">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNoTelponKantorIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.Telp. Kantor Ibu
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNoTelponKantorIbu"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtAlamatKantorIbu.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Alamat Kantor Ibu
						                        </label>
						                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAlamatKantorIbu" style="resize: none;"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 0px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Data Saudara Kandung
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.
						                        </label>
						                        <asp:TextBox Text="1" Enabled="false" ValidationGroup="vldInput" runat="server" ID="txtNoSaudara_1" CssClass="input-box" style="background-color: #1DA1F2; text-align: center; color: white; font-weight: bold; border-style: none; border-left-style: solid; border-left-color: #037ECB; border-left-width: 3px;"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboHubungan_1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hubungan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboHubungan_1" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="Kakak" Text="Kakak"></asp:ListItem>
                                                    <asp:ListItem Value="Adik" Text="Adik"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaLengkapSaudara_1" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboJenisKelamin_1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin_1" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="L" Text="Laki-Laki"></asp:ListItem>
                                                    <asp:ListItem Value="P" Text="Perempuan"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtUmurSaudara_1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Umur
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtUmurSaudara_1" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaSekolahSaudara_1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Sekolah / Kelas
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaSekolahSaudara_1" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtKeteranganSaudara_1.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Keterangan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtKeteranganSaudara_1" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: #1DA1F2; border-width: 2px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.
						                        </label>
						                        <asp:TextBox Text="2" Enabled="false" ValidationGroup="vldInput" runat="server" ID="txtNoSaudara_2" CssClass="input-box" style="background-color: #1DA1F2; text-align: center; color: white; font-weight: bold; border-style: none; border-left-style: solid; border-left-color: #037ECB; border-left-width: 3px;"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboHubungan_2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hubungan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboHubungan_2" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="Kakak" Text="Kakak"></asp:ListItem>
                                                    <asp:ListItem Value="Adik" Text="Adik"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaLengkapSaudara_2" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboJenisKelamin_2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin_2" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="L" Text="Laki-Laki"></asp:ListItem>
                                                    <asp:ListItem Value="P" Text="Perempuan"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtUmurSaudara_2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Umur
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtUmurSaudara_2" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaSekolahSaudara_2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Sekolah / Kelas
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaSekolahSaudara_2" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtKeteranganSaudara_2.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Keterangan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtKeteranganSaudara_2" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: #1DA1F2; border-width: 2px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.
						                        </label>
						                        <asp:TextBox Text="3" Enabled="false" ValidationGroup="vldInput" runat="server" ID="txtNoSaudara_3" CssClass="input-box" style="background-color: #1DA1F2; text-align: center; color: white; font-weight: bold; border-style: none; border-left-style: solid; border-left-color: #037ECB; border-left-width: 3px;"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboHubungan_3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hubungan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboHubungan_3" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="Kakak" Text="Kakak"></asp:ListItem>
                                                    <asp:ListItem Value="Adik" Text="Adik"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaLengkapSaudara_3" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboJenisKelamin_3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin_3" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="L" Text="Laki-Laki"></asp:ListItem>
                                                    <asp:ListItem Value="P" Text="Perempuan"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtUmurSaudara_3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Umur
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtUmurSaudara_3" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaSekolahSaudara_3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Sekolah / Kelas
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaSekolahSaudara_3" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtKeteranganSaudara_3.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Keterangan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtKeteranganSaudara_3" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: #1DA1F2; border-width: 2px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.
						                        </label>
						                        <asp:TextBox Text="4" Enabled="false" ValidationGroup="vldInput" runat="server" ID="txtNoSaudara_4" CssClass="input-box" style="background-color: #1DA1F2; text-align: center; color: white; font-weight: bold; border-style: none; border-left-style: solid; border-left-color: #037ECB; border-left-width: 3px;"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboHubungan_4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hubungan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboHubungan_4" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="Kakak" Text="Kakak"></asp:ListItem>
                                                    <asp:ListItem Value="Adik" Text="Adik"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaLengkapSaudara_4" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboJenisKelamin_4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin_4" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="L" Text="Laki-Laki"></asp:ListItem>
                                                    <asp:ListItem Value="P" Text="Perempuan"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtUmurSaudara_4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Umur
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtUmurSaudara_4" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaSekolahSaudara_4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Sekolah / Kelas
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaSekolahSaudara_4" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtKeteranganSaudara_4.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Keterangan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtKeteranganSaudara_4" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: #1DA1F2; border-width: 2px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.
						                        </label>
						                        <asp:TextBox Text="5" Enabled="false" ValidationGroup="vldInput" runat="server" ID="txtNoSaudara_5" CssClass="input-box" style="background-color: #1DA1F2; text-align: center; color: white; font-weight: bold; border-style: none; border-left-style: solid; border-left-color: #037ECB; border-left-width: 3px;"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboHubungan_5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hubungan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboHubungan_5" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="Kakak" Text="Kakak"></asp:ListItem>
                                                    <asp:ListItem Value="Adik" Text="Adik"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaLengkapSaudara_5" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboJenisKelamin_5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin_5" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="L" Text="Laki-Laki"></asp:ListItem>
                                                    <asp:ListItem Value="P" Text="Perempuan"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtUmurSaudara_5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Umur
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtUmurSaudara_5" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaSekolahSaudara_5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Sekolah / Kelas
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaSekolahSaudara_5" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtKeteranganSaudara_5.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Keterangan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtKeteranganSaudara_5" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: #1DA1F2; border-width: 2px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.
						                        </label>
						                        <asp:TextBox Text="6" Enabled="false" ValidationGroup="vldInput" runat="server" ID="txtNoSaudara_6" CssClass="input-box" style="background-color: #1DA1F2; text-align: center; color: white; font-weight: bold; border-style: none; border-left-style: solid; border-left-color: #037ECB; border-left-width: 3px;"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboHubungan_6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hubungan
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboHubungan_6" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="Kakak" Text="Kakak"></asp:ListItem>
                                                    <asp:ListItem Value="Adik" Text="Adik"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaLengkapSaudara_6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Lengkap
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaLengkapSaudara_6" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= cboJenisKelamin_6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Jenis Kelamin
						                        </label>
						                        <asp:DropDownList ValidationGroup="vldInput" runat="server" ID="cboJenisKelamin_6" CssClass="input-box">
                                                    <asp:ListItem></asp:ListItem>
                                                    <asp:ListItem Value="L" Text="Laki-Laki"></asp:ListItem>
                                                    <asp:ListItem Value="P" Text="Perempuan"></asp:ListItem>
						                        </asp:DropDownList>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-1">
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtUmurSaudara_6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Umur
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtUmurSaudara_6" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtNamaSekolahSaudara_6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama Sekolah / Kelas
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtNamaSekolahSaudara_6" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group form-group-label" style="margin-top: 10px; margin-bottom: 0px; margin-left: 0px;">
						                        <label for="<%= txtKeteranganSaudara_6.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Keterangan
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" runat="server" ID="txtKeteranganSaudara_6" CssClass="input-box"></asp:TextBox>
					                        </div>
                                        </div>
                                    </div>
                
                                    <div class="row">
                                        <div class="col-md-12">
                                            <hr style="margin: 0px; margin-top: 15px; border-color: #1DA1F2; border-width: 2px;" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group form-group-label" style="color: #1DA1F2; font-weight: bold; margin-top: 15px; padding-top: 10px; margin-bottom: 5px;">
						                        <i class="fa fa-hashtag"></i>
						                        Kontak Darurat
                                                <br />
                                                <span style="font-weight: normal;">
                                                    Dalam keadaan darurat yang dapat kami hubungi selain orang tua siswa adalah :
                                                </span>
					                        </div>
                                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNamaKontakDarurat.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Nama yang dihubungi
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNamaKontakDarurat"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
				                        <div class="col-md-6">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtHubunganKontakDarurat.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Hubungan dengan siswa
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtHubunganKontakDarurat"></asp:TextBox> 
					                        </div>
				                        </div>
                                        <div class="col-md-6">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtNoTelponHPKontakDarurat.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    No.Telp/HP
						                        </label>
						                        <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtNoTelponHPKontakDarurat"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
					                        <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                        <label for="<%= txtAlamatKontakDarurat.ClientID %>" style="color: #B7770D; font-size: small;">
                                                    Alamat
						                        </label>
						                        <asp:TextBox TextMode="MultiLine" Rows="3" ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtAlamatKontakDarurat" style="resize: none;"></asp:TextBox> 
					                        </div>
				                        </div>
                                    </div>

                                    <div runat="server" id="div_survey">
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-top: 15px;">
				                            <div class="col-md-12" style="font-weight: bold; color: #1DA1F2;">
                                                <br />
                                                <i class="fa fa-hashtag"></i>
                                                &nbsp;
					                            Informasi pengetahuan mengenai Al Izhar
                                                <hr style="margin-top: 10px; margin-bottom: 10px;" />
				                            </div>
                                        </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 0px; padding-bottom: 0px;">
                                            <div class="col-md-12" style="padding-top: 5px;">
                                                <span style="font-weight: bold;">Social Media</span>
				                            </div>
			                            </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 0px; padding-bottom: 15px;">
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkFacebook.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkFacebook.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkFacebook" type="checkbox" />
                                                                <i class="fa fa-facebook-official" style="color: #3C5A98;"></i>&nbsp;
										                        <span class="switch-toggle"></span>Facebook
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkInstagram.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkInstagram.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkInstagram" type="checkbox" />
                                                                <i class="fa fa-instagram" style="color: saddlebrown;"></i>&nbsp;
										                        <span class="switch-toggle"></span>Instagram
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkTwitter.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkTwitter.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkTwitter" type="checkbox" />
                                                                <i class="fa fa-twitter-square" style="color: dodgerblue;"></i>&nbsp;
										                        <span class="switch-toggle"></span>Twitter
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkWebsite.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkWebsite.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkWebsite" type="checkbox" />
                                                                <i class="fa fa-globe" style="color: black;"></i>&nbsp;
										                        <span class="switch-toggle"></span>Website
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkYoutube.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkYoutube.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkYoutube" type="checkbox" />
                                                                <i class="fa fa-youtube-play" style="color: red;"></i>&nbsp;
										                        <span class="switch-toggle"></span>Youtube
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                        </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 0px; padding-bottom: 0px;">
                                            <div class="col-md-12" style="padding-top: 5px;">
                                                <span style="font-weight: bold;">Media Cetak</span>
				                            </div>
			                            </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 0px; padding-bottom: 15px;">
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkFlyer.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkFlyer.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkFlyer" type="checkbox" />
										                        <span class="switch-toggle"></span>Flyer
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkSpanduk.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkSpanduk.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkSpanduk" type="checkbox" />
										                        <span class="switch-toggle"></span>Spanduk
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkBrosur.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkBrosur.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkBrosur" type="checkbox" />
										                        <span class="switch-toggle"></span>Brosur
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkBaliho.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkBaliho.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkBaliho" type="checkbox" />
										                        <span class="switch-toggle"></span>Baliho
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                        </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 0px; padding-bottom: 0px;">
                                            <div class="col-md-12" style="padding-top: 5px;">
                                                <span style="font-weight: bold;">Kegiatan Al Izhar (Kegiatan Sekolah)</span>
				                            </div>
			                            </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 0px; padding-bottom: 0px;">
                                            <div class="col-md-4">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkAtlas.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkAtlas.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkAtlas" type="checkbox" />
										                        <span class="switch-toggle"></span>Atlas
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkCare.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkCare.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkCare" type="checkbox" />
										                        <span class="switch-toggle"></span>CARE
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkSchoolVisit.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkSchoolVisit.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkSchoolVisit" type="checkbox" />
										                        <span class="switch-toggle"></span>School Visit
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                        </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 0px; padding-bottom: 15px;">
                                            <div class="col-md-4">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkAirGames.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkAirGames.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkAirGames" type="checkbox" />
										                        <span class="switch-toggle"></span>Air Games
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkAlizharCup.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkAlizharCup.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkAlizharCup" type="checkbox" />
										                        <span class="switch-toggle"></span>Al Izhar Cup
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group form-group-label" style="margin-top: 0px; margin-bottom: 5px;">
						                            <label for="<%= chkProgramParenting.ClientID %>" style="color: #B7770D; font-size: small;">
                                                        <div class="checkbox switch" style="padding-bottom: 0px;">
									                        <label for="<%= chkProgramParenting.ClientID %>" style="color: grey; font-weight: bold;">
										                        <input runat="server" class="access-hide" id="chkProgramParenting" type="checkbox" />
										                        <span class="switch-toggle"></span>Program Parenting
									                        </label>
								                        </div>
						                            </label>
					                            </div>
                                            </div>
                                        </div>
                                        <div class="row" style="border-left-style: solid; border-left-width: 3px; border-left-color: mediumvioletred; background-color: white; margin-left: 0px; margin-right: 0px; margin-bottom: 15px; padding-bottom: 15px;">
                                            <div class="col-md-12">
					                            <div class="form-group form-group-label" style="margin-top: 5px; margin-bottom: 5px;">
						                            <label for="<%= txtInfoAlizharLainnya.ClientID %>" style="color: black; font-size: small; font-weight: bold;">
                                                        Lainnya
						                            </label>
						                            <asp:TextBox ValidationGroup="vldInput" CssClass="input-box" runat="server" ID="txtInfoAlizharLainnya"></asp:TextBox> 
					                            </div>
				                            </div>
			                            </div>
                                    </div>

                                    <div runat="server" id="div_input_data">
					                    <div class="content-header ui-content-header" 
						                    style="background-color: #DA0379;
								                    box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                    background-image: none; 
								                    color: white;
								                    display: block;
								                    z-index: 5;
								                    position: fixed; bottom: 33px; right: 25px; width: 300px; border-radius: 25px;
								                    padding: 8px; margin: 0px; height: 35px;">
						                    <div style="padding-left: 15px;">
							                    <asp:LinkButton ToolTip=" List Data Siswa " runat="server" ID="btnBackToMenu" CssClass="btn-trans" OnClick="btnBackToMenu_Click" style="font-weight: bold; color: white;">
                                                    <i class="fa fa-list-alt"></i>
                                                    &nbsp;
                                                    Data Siswa
							                    </asp:LinkButton>
						                    </div>
					                    </div>

                                        <div class="content-header ui-content-header" 
						                    style="background-color: #00198d;
								                    box-shadow: 0 5px 6px rgba(0,0,0,0.16), 0 -2px 6px rgba(0,0,0,0.23); 
								                    background-image: none; 
								                    color: white;
								                    display: block;
								                    z-index: 5;
								                    position: fixed; bottom: 33px; right: 25px; width: 150px; border-radius: 25px;
								                    padding: 8px; margin: 0px; height: 35px;">
						                    <div style="padding-left: 15px;">
							                    <asp:LinkButton ValidationGroup="vldInput" OnClientClick="ShowProgress(Page_ClientValidate('vldInput'));" ToolTip=" Simpan Data " runat="server" ID="btnSaveData" CssClass="btn-trans" OnClick="btnSaveData_Click" style="font-weight: bold; color: white;">
                                                    <i class="fa fa-check"></i>
                                                    &nbsp;
								                    Simpan Data
							                    </asp:LinkButton>
						                    </div>
					                    </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-xs-12">
                                            &nbsp;
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </section>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
    <script type="text/javascript">
		InitPicker();
	</script>
</asp:Content>
