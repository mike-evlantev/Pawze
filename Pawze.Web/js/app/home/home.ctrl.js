angular.module('app').controller('HomeController', function ($scope, $timeout) {
    
 
    $timeout(function () {
        $('#homepageContainer .preloader').fadeOut(1000); // set duration in brackets   

        wow = new WOW(
    {
        mobile: false
    });
        wow.init();

        // ------- JQUERY PARALLAX ---- //
        function initParallax() {
            $('#homepageContainer #home').parallax("100%", 0.1);
            $('#homepageContainer #gallery').parallax("100%", 0.3);
            $('#homepageContainer #menu').parallax("100%", 0.2);
            $('#homepageContainer #team').parallax("100%", 0.3);
            $('#homepageContainer #contact').parallax("100%", 0.1);

        }
        initParallax();

        // HIDE MOBILE MENU AFTER CLIKING ON A LINK
        $('#homepageContainer .navbar-collapse a').click(function () {
            $("#homepageContainer .navbar-collapse").collapse('hide');
        });

        // NIVO LIGHTBOX
        $('#homepageContainer #gallery a').nivoLightbox({
            effect: 'fadeScale',
        });
    }, 1000);
    // ------- WOW ANIMATED ------ //
    
});