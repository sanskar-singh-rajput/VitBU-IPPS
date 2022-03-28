<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs"
    Inherits="SimpleTabControl.SimpleTabControl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <%--Script to Use Virtual Keyboard--%>
<head runat="server">
    <title>Capstone Project Team 8</title>
    <%--Script to disable clipboard copy by emtying its contents always--%>
    <script type="text/javascript">
        function clearData() {
            window.clipboardData.setData('text', '')
        }
        function cldata() {
            if (clipboardData) {
                clipboardData.clearData();
            }
        }
        setInterval("cldata();", 1000);
    </script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
 <link type="text/css" href="css/jquery.keypad.css" rel="stylesheet" />  
 <script type="text/javascript" src="javascript/jquery.plugin.min.js"></script>
 <script type="text/javascript" src="javascript/jquery.keypad.js"></script> 
    <script type="text/javascript">
        $(document).ready(function () {
            $("input[id$='chk_Keyboard']").click(function () {
                if (!$(this).is(':checked')) {
                    $('.inp_box').keypad('destroy');
                    $('.inp_box').keypad('destroy');
                } else {
                    $('.inp_box').keypad({ keypadOnly: true, layout: $.keypad.qwertyLayout, randomiseAlphabetic: true, randomiseNumeric: true, randomiseOther: true });
                    $('.inp_box').keypad({ keypadOnly: true, layout: $.keypad.qwertyLayout, randomiseAlphabetic: true, randomiseNumeric: true, randomiseOther: true });
                }
            });
        });
        function btnSubmit_onclick() {
        }
    </script>
    <style type="text/css">
        .Initial
        {
            display: block;
            padding: 4px 18px 4px 18px;
            float: left;
            background: url("../Images/InitialImage.png") no-repeat right top;
            color: Black;
            font-weight: bold;
        }
        .Initial:hover
        {
            color: White;
            background: url("../Images/SelectedButton.png") no-repeat right top;
        }
        .Clicked
        {
            float: left;
            display: block;
            background: url("../Images/SelectedButton.png") no-repeat right top;
            padding: 4px 18px 4px 18px;
            color: Black;
            font-weight: bold;
            color: White;
        }
        .auto-style1 {
            text-align: center;
        }
        .auto-style2 {
            color: #0033CC;
        }
        .auto-style3 {
            color: #0033CC;
            font-weight: normal;
        }
        .auto-style4 {
            font-weight: normal;
        }
        .auto-style5 {
            font-family: Constantia;
        }
        .auto-style6 {
            text-align: left;
        }
        .auto-style7 {
            width: 555px;
        }
        .auto-style8 {
            text-align: center;
            width: 934px;
        }
        .auto-style9 {
            width: 934px;
        }
        .auto-style10 {
            width: 435px;
        }
        .auto-style11 {
            font-family: Arial;
        }
        </style>
    
<link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Mukta+Malar:400,500,600'><link rel="stylesheet" href="./css/style.css">
<link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Mukta+Malar:400,500,600'><link rel="stylesheet" href="./css/invoice.css">
<link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Mukta+Malar:400,500,600'><link rel="stylesheet" href="./css/checkbox.css">
<link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Mukta+Malar:400,500,600'><link rel="stylesheet" href="./css/totp.css">


</head>

<body style="font-family: tahoma" ondragstart="return false;" onselectstart="return false;"  oncontextmenu="return false;" onload="clearData();" onblur="clearData();">
    <%--Script to disable print screen--%>
<script type="text/javascript">
    document.addEventListener("keyup", function (e) {
        var keyCode = e.keyCode ? e.keyCode : e.which;
        if (keyCode == 44) {
            stopPrntScr();
        }
    });
    function stopPrntScr() {

        var inpFld = document.createElement("input");
        inpFld.setAttribute("value", ".");
        inpFld.setAttribute("width", "0");
        inpFld.style.height = "0px";
        inpFld.style.width = "0px";
        inpFld.style.border = "0px";
        document.body.appendChild(inpFld);
        inpFld.select();
        document.execCommand("copy");
        inpFld.remove(inpFld);
    }
    function AccessClipboardData() {
        try {
            window.clipboardData.setData('text', "Access   Restricted");
        } catch (err) {
        }
    }
    setInterval("AccessClipboardData()", 300);
</script>
    <form id="form1" runat="server">
    <table width="50%" align="center">
        <tr>
            <td class="auto-style8">
                <h1 class="auto-style5">
                    <asp:Image ID="Image1" runat="server" ImageUrl="https://i.postimg.cc/RFkcf558/vit-bhopal-university-squarelogo-1630573537613-removebg-preview.png" />
                </h1>
                <h1 class="auto-style5"><span class="auto-style3">
                    VIT Bhopal University</span><span class="auto-style4"><br class="auto-style2" />
                    </span><span class="auto-style3">Payment Gateway</span></h1>

            </td>
        </tr>
        <tr>
            <td class="auto-style9">
                <asp:Button Text="VITBU Payment" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server"
                    OnClick="Tab1_Click" />
                <%--<asp:Button Text="Tab 2" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server"
                    OnClick="Tab2_Click" />
                <asp:Button Text="Tab 3" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server"
                    OnClick="Tab3_Click" />--%>
                <asp:MultiView ID="MainView" runat="server">
                    <asp:View ID="View1" runat="server">
                        <div align="center">
                            <table style="width: 100%;  border-width: 1px; border-color: #666; border-style: solid">
                            <tr>
                                <td class="auto-style7" style="text-align: center; vertical-align: top;">
                                    <h3>
                                        <span class="auto-style11">Payment Method:</span>
                                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" Height="18px" Width="193px" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                                            <asp:ListItem>Select method</asp:ListItem>
                                            <asp:ListItem>Credit Card</asp:ListItem>
                                            <asp:ListItem>Debit Card</asp:ListItem>
                                            <asp:ListItem>Google Pay</asp:ListItem>
                                            <asp:ListItem>Paytm Wallet</asp:ListItem>
                                            <asp:ListItem>PhonePe Wallet</asp:ListItem>
                                            <asp:ListItem>UPI</asp:ListItem>
                                        </asp:DropDownList>
                                    </h3>
                                    
                                    <asp:Panel ID="Panel1" runat="server" Height="192px">
                                        <center>
                                        <div class="form-group">
                                            <span>Credit Card Number</span>
                                            <asp:TextBox ID="TextBox1" class="inp_box" placeholder="1111 2222 3333 4444" runat="server" Width="230px" oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <span>Account Holder Name</span>
                                            <asp:TextBox ID="TextBox2" class="inp_box" placeholder="Full name" runat="server" Width="230px" oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <span>Security Code (CVV)</span>
                                            <asp:TextBox ID="TextBox3" class="inp_box" placeholder="123" runat="server" Width="230px" oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <span>Expiration Date</span>
                                            <asp:TextBox ID="TextBox4" class="inp_box" placeholder="M/YYYY" runat="server" Width="100%"  oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>
                                        </center>
                                    </asp:Panel>

                                    

                                    <asp:Panel ID="Panel2" runat="server" Height="192px">
                                        <center>
                                        <div class="form-group">
                                            <span>Debit Card Number</span>
                                            <asp:TextBox ID="TextBox5" class="inp_box" placeholder="1111 2222 3333 4444" runat="server" Width="230px" oncopy="return true" onpaste="return false" Text="6011 6568 1861 8269"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <span>Account Holder Name</span>
                                            <asp:TextBox ID="TextBox6" class="inp_box" placeholder="Arnaq Millary" runat="server" Width="230px" oncopy="return true" onpaste="return false" Text="Arnaq Millaray"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <span>Security Code (CVV)</span>
                                            <asp:TextBox ID="TextBox7" class="inp_box" placeholder="123" runat="server" Width="230px" oncopy="return true" onpaste="return false" Text="212"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <span>Expiration Date</span>
                                            <asp:TextBox ID="TextBox8" class="inp_box" placeholder="M/YYYY" runat="server" Width="100%" oncopy="return true" onpaste="return false" Text="3/2028"></asp:TextBox>
                                        </div>
                                            </center>
                                    </asp:Panel>

                                    <asp:Panel ID="Panel3" runat="server" Height="54px">
                                        <center>
                                        <div class="form-group">
                                            <span>Google Pay ID</span>
                                            <asp:TextBox ID="TextBox21" class="inp_box" placeholder="" runat="server" Width="230px" oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>
                                            </center>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel4" runat="server" Height="52px">
                                        <center>
                                        <div class="form-group">
                                            <span>PayTM Registered Phone Number</span>
                                            <asp:TextBox ID="TextBox13" class="inp_box" placeholder="" runat="server" Width="230px" oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>
                                            </center>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel5" runat="server" Height="47px">
                                        <center>
                                        <div class="form-group">
                                            <span>PhonePe Registered Phone Number</span>
                                            <asp:TextBox ID="TextBox17" class="inp_box" placeholder="" runat="server" Width="230px" oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>
                                            </center>
                                    </asp:Panel>
                                    <asp:Panel ID="Panel6" runat="server" Height="38px">
                                        <center>
                                        <div class="form-group">
                                            <span>UPI ID</span>
                                            <asp:TextBox ID="TextBox22" class="inp_box" placeholder="" runat="server" Width="230px" oncopy="return true" onpaste="return false"></asp:TextBox>
                                        </div>
                                            </center>
                                    </asp:Panel>

                                    
                                    


                                      </div>
                                    </div>

                                    
                                </td>
                                <td class="auto-style10" style="text-align: center; vertical-align: middle;">
                                    <center>
                                        
                                      <div id="Checkout" class="auto-style6" style="margin-top:5px; width:377px; margin-bottom:5px;">
                                          <h1>Merchant Details</h1>
                                          <div class="card-row">
                                              <span class="visa"></span>
                                              <span class="mastercard"></span>
                                              <span class="amex"></span>
                                              <span class="discover"></span>
                                          </div>

                                          

                                          <div class="auto-style1">
                                              <strong>
                                              <input type="checkbox" id="chk_Keyboard" />
                                              <span class="auto-style11">Use Virtual Keyboard</span>
                                              <br />
                                              </strong>
                                              <br />
                                              Transaction Amount: 1000 INR<br />
                                              <br />
                                              Merchant Name: Nanabah Naira<br /> Merchant ID: QuTLhX1tC9<br /> Currency System: USD<br />
                                              <br />
                                              <br />

                                              Time-based OTP:
                                              <asp:TextBox ID="TextBox23" runat="server"></asp:TextBox>
                                              
                                              <br />
                                              <br />
                                              <asp:Button ID="Button1" CssClass="button" runat="server" align="center" OnClick="Button1_Click" Text="Make Payment"/>
                                              <br />
                                          <br />
                                          </div>
                                        
                                           
                                        </div>
                                    </center>
                                    <br />
                                </td>

                            </tr>
                        </table>
                        </div>
                    </asp:View>
                    <%--<asp:View ID="View2" runat="server">
                        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    <h3>
                                        View 2
                                    </h3>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="View3" runat="server">
                        <table style="width: 100%; border-width: 1px; border-color: #666; border-style: solid">
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    <h3>
                                        View 3
                                    </h3>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </asp:View>--%>
                </asp:MultiView>
            </td>
        </tr>
    </table>
    </form>

</body>
</html>
