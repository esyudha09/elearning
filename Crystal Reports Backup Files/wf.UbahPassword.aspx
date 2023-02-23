<%@ Page Title="" Language="C#" MasterPageFile="~/Application_Masters/Main.Master" AutoEventWireup="true" CodeBehind="wf.UbahPassword.aspx.cs" Inherits="AI_ERP.wf_UbahPassword" %>
<%@ MasterType VirtualPath="~/Application_Masters/Main.Master" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgress" Src="~/Application_Controls/Res/PostbackUpdateProgress.ascx" %>
<%@ Register TagPrefix="ucl" TagName="PostbackUpdateProgressNoBG" Src="~/Application_Controls/Res/PostbackUpdateProgressNoBG.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function EndRequestHandler() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequest);
        }
        function EndRequest() {
            
        }
    </script>    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content-inner margin-top-no margin-bottom-no" style="margin-bottom: 0px;">
        <div class="row" style="margin-top: 45px; margin-bottom: 0px; margin-left: 0px; margin-right: 0px;">
            <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2">
				<div class="card margin-bottom-no margin-top-no" style=" background-color: #B93221; border-bottom-left-radius: 0px; border-bottom-right-radius: 0px;">
					<div class="card-inner" style="padding: 0px; margin: 7px; width: 100%;">
						<div style="width: 100%; margin-top: 0px; margin-bottom: 0px; padding: 0px;">

							<label style="font-weight: bold; color: white; font-size: large;">
								<div class="tile-wrap" style="margin: 0px;">
									<div class="tile" style="box-shadow: none; background: transparent;">
										<div class="tile-side pull-left">
											<div class="avatar avatar-sm" style="background-color: #DA4336;">
												<i class="fa fa fa-cog"></i>
											</div>
										</div>
										<div class="tile-inner" style="margin: 0px; margin-left: 10px; margin-top: 6px;">
											<span>
												Ubah Password
											</span>
                                            <br />
                                            <label style="font-weight: normal; font-size: small;">
                                                Untuk mengganti password, isi password lama dan password baru anda <br />
                                                <label style="color: yellow; font-weight: normal;"><i class="fa fa-exclamation-triangle"></i>&nbsp;&nbsp;Huruf besar dan kecil berpengaruh</label>
                                            </label>
										</div>
									</div>
								</div>
							</label>

						</div>
					</div>
				</div>
			</div>
        </div>
        <div class="row" style="margin-bottom: 15px; margin-top: 0px; margin-left: 0px; margin-right: 0px;">
		    <div class="col-lg-6 col-lg-offset-3 col-md-8 col-md-offset-2">
			    <div class="card margin-bottom-no" style="margin-top: 10px;">
				    <div class="card-inner" style="padding: 7px; margin: 0px; width: 100%;">
                        <section class="row" style="padding: 10px; margin: 0px; padding-bottom: 0px; color: #bfbfbf;">
                            <div runat="server" id="divMain" class="col-lg-12" style="padding-left: 0px; padding-right: 0px;">
			                    <section class="content-inner margin-top-no margin-bottom-no">
                                    <div style="margin-top: 0px; background-color: white;">
					                    <div style="padding: 0px; border-top-left-radius: 0px; border-top-right-radius: 0px;">
                                            <div style="padding: 0px; margin: 7px; margin-top: 0px;">
                                                <div style="padding: 20px; padding-top: 0px; margin-bottom: 0px; padding-bottom: 0px;">
                                                    <div runat="server" id="div_infosimpan" visible="false" style="margin: 0 auto; display: table; font-weight: bold; 
                                                                padding: 10px; border-bottom-left-radius: 5px; 
                                                                border-bottom-right-radius: 5px;
                                                                background-color: rgba(205, 203, 203, 0.30);">
                                                        <i class="fa fa-info-circle"></i>
                                                        &nbsp;&nbsp;
                                                        <asp:Label runat="server" ID="lblInfoSimpan"></asp:Label>
                                                    </div>
                                                    <div class="row" style="margin-top: 20px; padding-left: 15px; padding-right: 15px;">
                                                        <label for="<%= txtPasswordLama.ClientID %>">
                                                            <label for="<%= txtPasswordLama.ClientID %>" style="color: #7A894D; border-radius: 5px; font-weight: bold;">
                                                                <i class="fa fa-lock" aria-hidden="true"></i>
                                                                &nbsp;
                                                                Password Lama
                                                            </label>
                                                            <asp:RequiredFieldValidator 
                                                                runat="server" ID="valPasswordLama" 
                                                                Display="Dynamic" ForeColor="Red" 
                                                                ValidationGroup="input_setting_password" 
                                                                SetFocusOnError="true"
                                                                ControlToValidate="txtPasswordLama" 
                                                                Text="&nbsp;&nbsp;<i class='fa fa-exclamation-triangle'></i>&nbsp;&nbsp;Harus Diisi"></asp:RequiredFieldValidator>
                                                        </label>
                                                        <asp:TextBox ValidationGroup="input_setting_password" CssClass="form-control" TextMode="Password"
                                                            class="date-input" runat="server" ID="txtPasswordLama" style="font-weight: bold; padding-left: 20px; background-color: white; margin-bottom: 10px; margin-top: 10px; color: black;" />
                                                    </div>
                                                    <div class="row" style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">
                                                        <label for="<%= txtPasswordBaru.ClientID %>">
                                                            <label for="<%= txtPasswordBaru.ClientID %>" style="color: #7A894D; border-radius: 5px; font-weight: bold;">
                                                                <i class="fa fa-lock" aria-hidden="true"></i>
                                                                &nbsp;
                                                                Password Baru
                                                            </label>
                                                            <asp:RequiredFieldValidator 
                                                                runat="server" ID="valPasswordBaru" 
                                                                Display="Dynamic" ForeColor="Red" 
                                                                ValidationGroup="input_setting_password" 
                                                                SetFocusOnError="true"
                                                                ControlToValidate="txtPasswordBaru" 
                                                                Text="&nbsp;&nbsp;<i class='fa fa-exclamation-triangle'></i>&nbsp;&nbsp;Harus Diisi"></asp:RequiredFieldValidator>
                                                        </label>
                                                        <asp:TextBox ValidationGroup="input_setting_password" CssClass="form-control" TextMode="Password"
                                                            class="date-input" runat="server" ID="txtPasswordBaru" style="font-weight: bold; padding-left: 20px; background-color: white; margin-bottom: 10px; margin-top: 10px; color: black;" />
                                                    </div>
                                                    <div class="row" style="padding-left: 15px; padding-right: 15px; padding-top: 10px;">
                                                        <label for="<%= txtPasswordBaruTulis.ClientID %>">
                                                            <label for="<%= txtPasswordBaruTulis.ClientID %>" style="color: #7A894D; border-radius: 5px; font-weight: bold;">
                                                                <i class="fa fa-lock" aria-hidden="true"></i>
                                                                &nbsp;
                                                                Tulis Ulang Password Baru
                                                            </label>
                                                            <asp:RequiredFieldValidator 
                                                                runat="server" ID="valTulisPasswordBaru" 
                                                                Display="Dynamic" ForeColor="Red" 
                                                                ValidationGroup="input_setting_password" 
                                                                SetFocusOnError="true"
                                                                ControlToValidate="txtPasswordBaruTulis" 
                                                                Text="&nbsp;&nbsp;<i class='fa fa-exclamation-triangle'></i>&nbsp;&nbsp;Harus Diisi"></asp:RequiredFieldValidator>
                                                        </label>
                                                        <asp:TextBox ValidationGroup="input_setting_password" CssClass="form-control" TextMode="Password"
                                                            class="date-input" runat="server" ID="txtPasswordBaruTulis" style="font-weight: bold; padding-left: 20px; background-color: white; margin-bottom: 10px; margin-top: 10px; color: black;" />
                                                    </div>
                                                    <br />
                                                    <div class="row" style="padding-left: 15px; padding-right: 15px;">
                                                        <asp:LinkButton ValidationGroup="input_setting_password" OnClick="lnkSaveAkun_Click" runat="server" ID="lnkSaveAkun" CssClass="btn btn-info pull-right" style="font-weight: bold; background-color: #446D8C; outline: none; border-color: #446D8C; color: white; margin-bottom: 20px;">
                                                            &nbsp;&nbsp;
                                                            <i class="fa fa-check" aria-hidden="true"></i>
                                                                &nbsp;
                                                            Simpan
                                                            &nbsp;&nbsp;
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </section>
                            </div>
                        </section>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="KontenBawah" runat="server">
</asp:Content>
