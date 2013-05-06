// https://bugzilla.xamarin.com/show_bug.cgi?id=3143#c10
// fix mono tab bug
jQuery(document).ready(function () {
	jQuery("textarea").each(function(){
	   this.value = this.value.trim();
	});
})
