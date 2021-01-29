$(document).ready(function ()
{
    ///所属公司
    $("#Contract_CompanyName").val($("#Contract_CompanyCode").find("option:selected").text());  //初次进入时获取选中项
    $("#Contract_CompanyCode").change
        (function () {
            $("#Contract_CompanyName").val($("#Contract_CompanyCode").find("option:selected").text())  //选中项变化时获取选中项
        }
        );

    ///业务大类
    $("#Contract_BusinessName").val($("#Contract_BusinessCode").find("option:selected").text());  //初次进入时获取选中项
    $("#Contract_BusinessCode").change
        (function () {
            $("#Contract_BusinessName").val($("#Contract_BusinessCode").find("option:selected").text())  //选中项变化时获取选中项
        }
        );

    ///部门名称
    $("#Contract_DepartmentName").val($("#Contract_DepartmentCode").find("option:selected").text());  //初次进入时获取选中项
    $("#Contract_DepartmentCode").change
        (function () {
            $("#Contract_DepartmentName").val($("#Contract_DepartmentCode").find("option:selected").text())  //选中项变化时获取选中项
        }
        );

    

    ///合同种类
    $("#Contract_ContractTypeName").val($("#Contract_ContractTypeCode").find("option:selected").text());  //初次进入时获取选中项
    $("#Contract_ContractTypeCode").change
        (function () {
            $("#Contract_ContractTypeName").val($("#Contract_ContractTypeCode").find("option:selected").text())  //选中项变化时获取选中项
        }
        );

    ///返回按钮操作
    $('#returnButton').live('click', function () {
        window.history.back();
    });

    $('#submitButton').live('click', function ()
    {
        if ($('#Contract_ContractStartDate').val() == "") {
            alert("合同起始日期为空！")
            return false;
        }
        else {
            return true;
        }
    })




});