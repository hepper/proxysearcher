$("#download-page").ready(function () {
    $("#download-right-now").click(function (a) {
        $("#menu-item-download").click();
    });
});

$(".menu-element").ready(function () {
    var menuItems = $('.menu-item');
    menuItems.click(function () {
        var clickedItem = this;
        menuItems.each(function (index) {
            var currentItem = $(menuItems[index]);
            var currentPanel = $($(currentItem.find("input")[0]).val());

            if (menuItems[index] == clickedItem) {
                currentItem.addClass("menu-item-active");
                currentItem.removeClass("menu-item-selected");
                currentPanel.show();
                currentPanel.trigger("isVisible");
            }
            else {
                currentItem.removeClass("menu-item-active");
                currentPanel.trigger("isHide");
                currentPanel.hide();
            }
        });
    });

    menuItems.hover(function () {
        if (!$(this).hasClass("menu-item-active")) {
            $(this).addClass("menu-item-selected");
        }
    }, function () {
        
        $(this).removeClass("menu-item-selected");
    })
});

$(".current-version").ready(function () {
    var getVersion = function () {
        versionNumber = parseInt($("#currentVersion").val());
        var minor = versionNumber % 10;
        var major = versionNumber - versionNumber % 10 + 1;

        return major + "." + minor;
    }

    $("#version-span").text(getVersion());
});

$("#download-page").ready(function () {
    var interval;

    $("#download-page").bind("isVisible", function () {
        var timeLeftId = $("#time-left-id");
        var url = $("#installationPath").val();

        timeLeftId.text(3);

        $("#download-direct-link").attr("href", url);
        $("#download-direct-link").click(function (e) {
            e.preventDefault();
            window.location.href = url;
            return false;
        });

        var handler = function () {
            var value = parseInt(timeLeftId.text());

            if (value == 0) {
                clearInterval(interval);
                $("#download-direct-link").click();
            }
            else {
                value--;
                timeLeftId.text(value);
            }
        }

        interval = setInterval(handler, 1000);
    });

    $("#download-page").bind("isHide", function () {
        clearInterval(interval);
    });
});

$(".screenshoot1-link").ready(function () {
    $(".screenshoot1-link").fancybox({
        'transitionIn': 'elastic',
        'transitionOut': 'elastic',
        'speedIn': 600,
        'speedOut': 200,
        'overlayShow': false
    });
});