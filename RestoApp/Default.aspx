<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Default.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RestoApp.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- LOGIN-->
    <div class="container">
        <div class="d-flex flex-column mt-5 align-items-center vh-100">
            <img src="/Content/Image/logo.jpg" style="width: 200px; height: 200px;" class="rounded-circle mb-3" />
            <div class="col-sm-5 bg-body-secondary p-3 border rounded-3">

                <h2 class="fw-bold fs-4 text-center my-3"><%: !esRecuperarPass ? "LOGIN" : "RECUPERAR CONTRASEÑA"%> <%:esRecuperarPass && token != null ? mail : ""%></h2>

                <div class="mb-3 row">
                    <%if (token == null)
                        { %>
                    <label for="staticEmail" class="col-sm-2 col-form-label">Email</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txb_Usuario" type="text" class="form-control" placeholder="email@example.com" />
                    </div>
                </div>
                <%} %>
                <%if (!esRecuperarPass || (esRecuperarPass && token != null))
                    {  %>
                <div class="mb-3 row">
                    <label for="inputPassword" class="col-sm-2 col-form-label">Password</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txb_Password" type="password" class="form-control" placeholder="******" />
                    </div>
                </div>

                <%} %>
                 <%if (esRecuperarPass && token != null)
                    {  %>
                <div class="mb-3 row">
                    <label for="inputPassword" class="col-sm-2 col-form-label">Confirmar Password</label>
                    <div class="col-sm-10">
                        <asp:TextBox runat="server" ID="txb_PasswordConfirm" type="password" class="form-control" placeholder="******" OnTextChanged="txb_PasswordConfirm_TextChanged" />
                    </div>
                </div>

                <%} %>
                <div class="d-flex justify-content-center">
                    <asp:Button Text="Acceder" ID="btnForm" runat="server" OnClick="EnviarDatos_Click" CssClass="btn btn-dark btn-block" />
                </div>
                <div class="d-flex flex-column justify-content-center">
                    <asp:Button ID="btnRecuperarPassword" runat="server" CssClass="text-end mt-3 text-gray-600 btn d-inline" Text ="¿Olvidaste tu contraseña?" OnCLick="RecuperarPass_Click"/>
                    <asp:Label CssClass="text-danger text-center" runat="server" ID="lbl_error" />
                </div>
            </div>
        </div>
    </div>
    <!-- FIN LOGIN-->

     <!--TOAST-->
    <div id="toastMesas" class="toastMain">
        <div id="toast-content" class="toast-content">
        </div>
    </div>
     <!--FIN TOAST-->

    <!-- ESTILOS -->
    <style>
        
        .toast-content{
            background-color: #fff;
            min-width: auto;
            min-height: auto;
            border-radius: 5px;
            border: #fff 3px solid;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.3);
        margin-top: 10px;
        }

        .toastMain{
            display: none;
            justify-content: center;
            align-items: center;
            position: fixed;
            z-index: 1;
            right: 50%;
            transform: translateX(50%);
            top: 0;
            background: rgba(0, 0, 0, 0);
            transition: 0.3s all ease;
        }

        .toastMessage{
            color: #666;
            font-size: 14px;
            font-weight: 600;
            padding: 5px 10px;
            margin: 0;
        }

        .toastMessage i {
            font-size: 16px;
            margin-right: 8px;
        }

        .iconSuccess{
            color: #66cd00;
        }

        .iconError{
            color: #c92a2a;
        }

        .opcaityToast {
              animation: fadein 0.5s, fadeout 0.5s 3.5s;
        }

        @keyframes fadein {
            from {opacity: 0; top: -20px;}
            to {opacity: 1; top: 0px;}
        }

        @keyframes fadeout {
            from {opacity: 1; top: 0px;}
            to {opacity: 0; top: -20px;}
        }
    </style>
    <!-- FIN ESTILOS -->

    <!-- SCRIPT -->
    <script id="scriptDefault">
        const toast = document.getElementById("toastMesas");
        let contenidoToast = document.getElementById("toast-content");

        function alertaToast(msg, mode) {
            toast.style.display = "flex";
            toast.classList.add("opcaityToast");
            contenidoToast.innerHTML = `<p class="toastMessage"><i class="fa-solid fa-circle-${mode != "error" ? "check iconSuccess" : "exclamation iconError"}"></i>${msg}</p>`

            //Tiempo para ocultar toast
            setTimeout(() => {
                toast.classList.remove("opcaityToast")
                toast.style.display = "none"
            }, 4000)
        }
    </script>
    <!-- FIN SCRIPT -->

</asp:Content>
