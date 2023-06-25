 /**=====================
     Quantity 2 js
==========================**/
 $(".addcart-button").click(function () {
     $(".add-to-cart-box .qty-input").val('0');
 });

 $('.add-to-cart-box').on('click', function () {
     let $qty = $(this).siblings(".qty-input");
     let currentVal = parseInt($qty.val());
     if (!isNaN(currentVal)) {
         $qty.val('0');
     }
 });

 $('.qty-left-minus').on('click', function () {
     let $qty = $(this).siblings(".qty-input");
     let currentVal = parseInt($qty.val());
     if (!isNaN(currentVal) && currentVal > 0) {
         $qty.val(currentVal - 1);
     }
 });

 $('.qty-right-plus').click(function () {
     $(this).prev().val(+$(this).prev().val() + 1);
 });