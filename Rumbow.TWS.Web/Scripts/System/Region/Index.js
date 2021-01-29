var regionId;
var setting = {
    async: {
        enable: true,
        url: "/System/Region/GetChildRegions",
        autoParam: ["id"]
    },
    callback: {
        beforeAsync: beforeAsync,
        onClick: onClick
    },
    view: {
        showIcon: true,
        showLine: true
    }
};

function beforeAsync(treeId, treeNode) {
    return true;
}

function onClick(e, treeId, node) {
    $('#regionId').val(node.id);
    ClickedRegion.innerHTML = node.name;
}

$(document).ready(function () {
    $.fn.zTree.init($("#treeDemo"), setting);

    $('#submitButton').click(function () {
        var rName = $("#RegionName").val();
        var rGrade = $("#Grade").val();
        if (rName == "" || rName == null) {
            Runbow.TWS.Alert("名称必填");
            return false;
        }

        if (isNaN(rGrade)) {
            Runbow.TWS.Alert("级别为数字");
            return false;
        }

        if ($('#regionId').val() === "") {
            $('#regionId').val("1");
        }
        return true;
    });
});