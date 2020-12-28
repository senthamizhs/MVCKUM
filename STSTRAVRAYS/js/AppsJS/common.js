
function showlobiboxalert(Alrttype, Msg) {
    
    try{
        Lobibox.alert(Alrttype, {   //eg: 'error' , 'success', 'info', 'warning'
            msg: Msg,
            closeOnEsc: false,
            callback: function ($this, type) {
            }
        });
    } catch (e) {
        console.log(e);
        alert(Msg);
    }
}


