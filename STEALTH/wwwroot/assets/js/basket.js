
let inps = document.querySelectorAll('#Basket tbody tr input');
let tbody = document.querySelector('tbody');

async function Change() {
    var itemId = this.closest('tr').dataset.id;
    var newQuantity = this.value;
    console.log(itemId, newQuantity);
    await fetch(`/Basket/UpdateQuantity/${itemId}/${newQuantity}`)
        .then(response => response.text())
        .then(data => {
            var resultDiv = document.getElementById('Basket');
            resultDiv.innerHTML = data;
            inps = document.querySelectorAll('#Basket tbody tr input')
            inps.forEach(element => {
                element.addEventListener('change', Change);
            })
        })
        .catch(error => console.error('Hata:', error));
    
}

inps.forEach(element => {
    element.addEventListener('change', Change);
})

tbody.querySelectorAll('tr i').forEach(e => {
    e.addEventListener('click', () => {
        let tr = e.parentElement.parentElement;
        let id = tr.getAttribute("data-id")
        fetch(`/Basket/DeleteBasket/${id}`)
        tr.remove()
    })
})

//$("tbody .anadi-product-quantity input").on('change', function () {
//    var itemId = $(this).closest('tr').data('id');
//    var newQuantity = $(this).val();
//    console.log(itemId, newQuantity);

//    fetch(`/Cart/UpdateQuantity/${itemId}/${newQuantity}`, {
//        method: 'POST',
//        headers: {
//            'Content-Type': 'application/json'
//        },
//    })



//})