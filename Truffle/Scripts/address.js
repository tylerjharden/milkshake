(function () {
    // check if we already have wishlu defined, is so, don't initialize again
    wishlu = window.wishlu || {};
    if (wishlu.version) {
        return;
    }

    // define wishlu version
    wishlu.version = '1.0.0.0.';

    // load jQuery async
    (function () {
        function loadScripts() {
            var s = document.createElement('script');
            s.type = 'text/javascript';
            s.async = true;
            s.src = "https://code.jquery.com/jquery-2.1.1.min.js";
            var x = document.getElementsByTagName('script')[0];
            x.parentNode.insertBefore(s, x);
        }
        window.attachEvent ? window.attachEvent('onload', loadScripts) : window.addEventListener('load', loadScripts, false);
    })();
    
    //alert("address.js loaded via script_tag!!!");

    // hook for wishlu purchase button on shopify product page
    wishlu.purchase = function () {     
        alert("clicked!");
    }

    $(document).ready(function () {
        if ($('.wishlu-share').length == 0)
            $("#social").append("<div class='wishlu-share' style='display: inline-block; height: 20px; width: 55px; padding-top: 4px; font-weight: bold; text-align: center; border-radius: 4px; border: 2px solid lightblue; background: #ddd; color: white;'><img src='http://dev.wishlu.com/Images/centerLogo.png' style='width: 16px; height: 16px; margin-right: 3px;' /><span style='position: relative; top: -2px;'>add</span></div>");

        // we are on a product page
        if ($('#add-to-cart').length > 0 && $("#buy-as-gift-wishlu").length == 0) {
            // onclick='wishlu.purchase();'
            
            var pid;

            if (__st)
                pid = __st.rid;

            $('#add-to-cart').parent().append("<a id='buy-as-gift-wishlu' class='btn addtocart' name='buy_wishlu' href='/apps/wishlu/give/" + pid + "'>Give as gift on wishlu</a>")
        }
    });

    window.wishlu = wishlu;
})();