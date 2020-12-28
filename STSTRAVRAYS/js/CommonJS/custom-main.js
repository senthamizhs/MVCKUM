/******* Vertical Tabs (Home page) *******/
//Change div on radio button click
$(function() {
	//flights 
	var $trips = $('.oneway,.roundtrip,.multicity');
	$trips.eq(0).show()
	  $('.tab input.input-tabs[type=radio]').on('change',function() {
		  $trips.hide();
		  $trips.eq( $('.tab input.input-tabs[type=radio]').index( this ) ).show();
	 });
});


 $('.trips input').each(function(){

    $(this).focus(function(){
      $(this).addClass('input-focus');
    });

    $(this).blur(function(){
      $(this).removeClass('input-focus');
    });

  });