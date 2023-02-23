<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostBackUpdateProgressNoBG.ascx.cs" Inherits="AI_ERP.Application_Controls.Res.PostBackUpdateProgressNoBG" %>
<div style="background: transparent; position: fixed; left: 0px; top: 0px; bottom: 0px; right: 0px; z-index: 9999999999">
    <div class="loder-box" style="background: rgba(0, 0, 0, 0.6); color: white; height: 170px; width: 170px; border-radius: 15px; top: 40%; padding-top: 25px;">
        <div class="progress-circular progress-circular-white">
            <div class="progress-circular-wrapper" style="margin: 0 auto; display: table;">
                <div class="progress-circular-inner">
                    <div class="progress-circular-left">
                        <div class="progress-circular-spinner"></div>
                    </div>
                    <div class="progress-circular-gap"></div>
                    <div class="progress-circular-right">
                        <div class="progress-circular-spinner"></div>
                    </div>
                </div>
            </div>
        </div>
        <label style="color: white; margin: 0 auto; display: table;">
            Sedang Proses...
        </label>
    </div>
</div>