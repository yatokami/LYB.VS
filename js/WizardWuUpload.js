//$(window).load(function () {
//});

//依傳入的自訂編號(fileUploadNum)，決定要執行哪個上傳控件，的上傳或刪除
jQuery.fn.loadUploadContent = function (fileUploadNum) {
    //调用wrap方法，为id为demo的div外层添加form元素，指定enctype为文件类型，action指定为asp.net文件
    $("#divUploadArea" + fileUploadNum).wrap("<form id='UploadForm" + fileUploadNum + "' action='../WizardWuUpload.ashx' method='post' enctype='multipart/form-data'></form>");

    //var bar = $('.bar');                  //获取上传进度条span
    var bar = $('#spanUploadBar' + fileUploadNum);

    //var percent = $('.percent');          //获取显示上传百分比的span
    var percent = $('#spanUploadPercent' + fileUploadNum);

    //var showimg = $('#showimg');          //显示图片的div
    //var showimg = $('#divShowImageAfterUploadSuccess' + fileUploadNum);

    //var progress = $(".progress");        //显示进度的div
    var progress = $("#divUploadProgress" + fileUploadNum);

    //var files = $(".files");              //文件上传控件input元素
    //var files = $("#divShowContentAfterUploadSuccess" + fileUploadNum);

    //var btn = $(".btn span");             //按钮文本
    var btn = $('#spanUploadSelect' + fileUploadNum);

    $("#WizardWuFileUpload" + fileUploadNum).change(function () {  //当上传文件改变时，触发事件 (不必按鈕，使用者選完圖片，就直接執行上傳的動作)
        $("#UploadForm" + fileUploadNum).ajaxSubmit({         //调用jquery.form插件的ajaxSubmit异步地提交表单
            dataType: 'json',             //返回数据类型为json
            beforeSend: function () {     //发送数据之前，执下的代码
                showimg.empty();          //清空图片预览区
                progress.show();          //显示进度
                var percentVal = '0%';    //显示进度百分比
                bar.width(percentVal);    //设置进度的宽度，增涨进度
                percent.html(percentVal); //设置进度值
                btn.html("上传中...");    //指定显示中
            },
            //更新进度条事件处理代码
            uploadProgress: function (event, position, total, percentComplete) {
                var percentVal = percentComplete + '%';
                bar.width(percentVal)     //更新进度条的宽度
                percent.html(percentVal);
            },
            success: function (data) {    //图片上传成功时
                //获取服务器端返回的文件数据
                //alert('success~');

                if (data.Result == "1") { //回傳 1 代表上傳成功，就顯示:預覽圖、刪除超連結
                    //返回 json 的取值方式
                    //alert(data.Name + "," + data.NewName + "," + data.Size + "," + data.Result);
                    files.html("<b>" + data.Name + "(" + data.Size + "KB)</b> <span class='DelImg' id='spanDelImg" + fileUploadNum + "' relNewName='" + data.NewName +
                        "' relName='" + data.Name + "'>删除图片</span>");   //显示:已经上传的文件名、刪除字樣			    

                    //var img = "files/"+data.NewName;            //得到文件路径
                    //alert(data.NewName);
                    
                    var img = "../uploadFiles/" + data.Name;        //得到文件路径(用來預覽已上傳的圖片, 原圖)
                    //var img = "../uploadFiles/" + data.NewName;   //得到文件路径(用來預覽已上傳的圖片, 截圖後的新圖)
                    //var img = "files/150128-01_cut.jpg";          //得到文件路径(用來預覽已上傳的圖片)

                    showimg.html("<img src='" + img + "'>");      //显示上传的图片预览 (顯示圖片的原始尺寸)
                    //showimg.html("<img src='" + img + "' style='float:left;padding:5px 5px;width:130px;height:130px'>"); //显示上传的图片预览 (控制圖片的顯示尺寸)
                }
                else if (data.Result == "-2") {
                    bar.width('0');                //重置进度条(會隱藏進度條)
                    alert('上传动作中止！\r\n\r\n不能上传 0 KB 的图片。');
                    files.html("上传动作中止！不能上传 0 KB 的图片。");
                }
                else if (data.Result == "-3") {
                    bar.width('0');                //重置进度条(會隱藏進度條)
                    alert('上传动作中止！\r\n\r\n不能上传大于 1 MB 的图片。');
                    files.html("上传动作中止！不能上传大于 1 MB 的图片。");
                }
                else if (data.Result == "-4") {
                    bar.width('0');                //重置进度条(會隱藏進度條)
                    alert('上传动作中止！\r\n\r\n只能上传图片格式的文件。');
                    files.html("上传动作中止！只能上传图片格式的文件。");
                }
                else if (data.Result == "0") {
                    bar.width('0');                //重置进度条(會隱藏進度條)
                    alert('上传失败！\r\n\r\n上传路径或后台 .NET 代码发生错误');
                    files.html("上传失败！上传路径或后台 .NET 代码发生错误。");
                }

                btn.html("选择文件");                   //上傳成功後，按鈕要顯示的新字樣
                //btn.html("选择文件"+fileUploadNum);
            },
            error: function (xhr, errorMsg, errorThrown) {  //图片上传失败时 (後端.NET錯誤:回傳型別錯誤會進入此error區塊)
                alert('上传时发生错误');
                //btn.html("上传时发生错误");  //按鈕要顯示的新字樣
                bar.width('0');                //重置进度条(會隱藏進度條)
                //files.html(xhr.responseText); //显示错误文本。(显示已经上传的文件名)
                alert(xhr.responseText);    //「System.String[]」
                //alert(errorMsg);    //「parsererror」
                //alert(errorThrown);     //「SyntaxError: 字元無效」
                //alert(xhr); //object型別
            }
        });
    });

    $("#spanDelImg" + fileUploadNum).live('click', function () {  //为删除按钮关联事件处理代码，这里用了live
        var picNewName = $(this).attr("relNewName");    //得到图片路径 (截圖後的新圖)
        var picName = $(this).attr("relName");          //得到图片路径 (原圖)
        //向服务器发送删除请求
        $.post("../WizardWuUpload.ashx?act=delimg", { NewNameDelete: picNewName, NameDelete: picName }, function (msg) {
            if (msg == "1") {
                files.html("删除成功");
                alert("删除成功");
                showimg.empty();
                progress.hide();            //重置进度条，并清空图片预览
            }
            else if (msg == "-5") {
                files.html("删除失败！文件不存在。");
                alert("删除失败！文件不存在。");
            }
            else if (msg == "0") {
                files.html("删除失败！删除时发生错误。");
                alert("删除失败！删除时发生错误。");
            } else {
                files.html("删除失败");
                alert("删除失败！");
            }
        });
    });

} //end of "jQuery.fn.loadUploadContent = function (fileUploadNum)"