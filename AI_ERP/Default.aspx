<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AI_ERP.Default" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Al-Izhar Pondok Labu</title>
	<meta charset="UTF-8">
	<meta content="IE=edge" http-equiv="X-UA-Compatible">
	<meta content="initial-scale=1.0, maximum-scale=1.0, user-scalable=no, width=device-width" name="viewport">

	<!-- css -->
	<link href="<%= ResolveUrl("~/Application_Templates/material-master/css/base.min-20200113h.css") %>" rel="stylesheet">
	<link href="<%= ResolveUrl("~/Application_Templates/material-master/css/project.min.css") %>" rel="stylesheet">
	<link href="<%= ResolveUrl("~/Application_CLibs/font-awesome/css/font-awesome.min.css") %>" rel="stylesheet">
</head>
<body style="background-color: white;" onload="<%= txtLoginUserID.ClientID %>.focus();">
	<form id="form1" runat="server">
		<div style="background-color: white;">
			<header class="header header-brand ui-header" style="background-color: #46545C; box-shadow: 0 1px 10px rgba(0, 0, 0, 0.5); display: none;">
				<div style="margin: 0 auto; display: table;">
					<span class="header-logo">
						<asp:Image runat="server" ID="imgLogo" ImageUrl="~/Application_Templates/logo.png" />
						&nbsp;&nbsp;
					    <span style="font-weight: bold;">Al-Izhar </span>
						&nbsp;
					    Pondok Labu
					</span>
				</div>
			</header>
			<main class="content" style="background-image: none; background-color: white;">
				<div class="container" style="background-image: none; background-color: white; margin-left: auto; margin-right: auto;">
                    <div class="row">
						<div class="col-lg-4 col-lg-push-4 col-sm-6 col-sm-push-3">
							<section class="content-inner">
								<div class="card" style="border-radius: 10px;">
									<div class="card-main">
										<div class="card-header">                                            
											<div class="card-inner">
												<center>
                                                    <div style="margin: 0 auto; display: table; width: 50px; height: 50px; background-color: #1A73E8; padding-top: 10px; border-radius: 100%; margin-bottom: 5px;">
                                                        <asp:Image runat="server" ID="imgAtas" ImageUrl="~/Application_Templates/logo.png" style="margin: 0 auto; display: table; height: 40px; width: 28px; margin-top: -5px;" />
                                                    </div>
												    <h1 class="card-heading" style="color: black; font-size: 14pt;">Al-Izhar Pondok Labu</h1>
											    </center>
											</div>
										</div>
										<div class="card-inner">
											<p class="text-center">
												<span class="avatar avatar-inline avatar-lg" style="background-color: white; ">
													<i class="fa fa-unlock-alt fa-4x" style="color: grey; margin-top: 15px;"></i>
												</span>
											</p>
											<div class="form-group form-group-label">
												<div class="row">
													<div class="col-md-10 col-md-push-1">
														<label for="<%= txtLoginUserID.ClientID %>" class="floating-label">User ID</label>
														<asp:RequiredFieldValidator ValidationGroup="vldLogin" runat="server" ID="vldUserName"
															ControlToValidate="txtLoginUserID" Display="Dynamic" Style="float: right; font-weight: bold;"
															Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
														<asp:TextBox ValidationGroup="vldLogin" CssClass="form-control" runat="server" ID="txtLoginUserID"></asp:TextBox>
													</div>
												</div>
											</div>
											<div class="form-group form-group-label">
												<div class="row">
													<div class="col-md-10 col-md-push-1">
														<label for="<%= txtLoginPassword.ClientID %>" class="floating-label">Password</label>
														<asp:RequiredFieldValidator ValidationGroup="vldLogin" runat="server" ID="vldPassword"
															ControlToValidate="txtLoginPassword" Display="Dynamic" Style="float: right; font-weight: bold;"
															Text="<i class='fa fa-exclamation-triangle'></i>" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
														<asp:TextBox ValidationGroup="vldLogin" CssClass="form-control" runat="server" ID="txtLoginPassword" TextMode="Password"></asp:TextBox>
													</div>
												</div>
											</div>
											<div class="form-group">
												<div class="row">
													<div class="col-md-10 col-md-push-1">
														<asp:LinkButton ValidationGroup="vldLogin" OnClick="btnLogin_Click" runat="server" ID="btnLogin" class="btn btn-block btn-brand waves-attach waves-light" Style="background-color: #1A73E8; font-weight: bold; text-transform: none;">
                                                            Masuk
														</asp:LinkButton>
													</div>
												</div>
											</div>
											<div class="form-group form-group-label">
												<div style="margin: 0 auto; display: table;">
													<asp:Label runat="server" ID="lblErrLogin" Visible="false" ForeColor="Red"
														Font-Bold="true"></asp:Label>
												</div>
											</div>
										</div>
									</div>
								</div>
								<div class="clearfix">
								</div>
							</section>
						</div>
					</div>
				</div>
			</main>
			<footer class="ui-footer" style="background-color: white; border-style: none;">
				<div class="container" style="margin-left: auto; margin-right: auto; font-size: smaller;">
					<p>Copyright &copy; <%= DateTime.Now.Year.ToString() %> Al-Izhar Pondok Labu</p>
				</div>
			</footer>
		</div>
	</form>

	<script src="<%= ResolveUrl("~/Application_CLibs/js/jquery.js") %>"></script>
	<script src="<%= ResolveUrl("~/Application_Templates/material-master/js/base.min.js") %>"></script>
	<script src="<%= ResolveUrl("~/Application_Templates/material-master/js/project.min.js") %>"></script>
</body>
</html>
