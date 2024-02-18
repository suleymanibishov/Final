let filter = document.getElementById('Filter');
let closebtn = filter.querySelector('.header svg');
let openbtn = document.querySelector('#Helpful-resources .filter-by .topics');
let showbtn = filter.querySelector('.footer a');
let opt = filter.querySelector('.main .row .active');

openbtn.addEventListener('click',() => filter.style.display = "flex");
closebtn.addEventListener('click', () => filter.style.display = "none");
function ResetEvent(data) {
    var tDiv = document.createElement("div");
    tDiv.innerHTML = data;
    document.getElementById('Helpful-resources').innerHTML = tDiv.querySelector('#Helpful-resources').innerHTML;;
    filter.innerHTML = tDiv.querySelector('#Filter').innerHTML
    closebtn = filter.querySelector('.header svg');
    openbtn = document.querySelector('#Helpful-resources .filter-by .topics');
    openbtn.addEventListener('click', () => filter.style.display = "flex");
    closebtn.addEventListener('click', () => filter.style.display = "none");
    filter.querySelectorAll('.main .row .opt').forEach(e => {
        e.addEventListener('click', () => {
            filter.querySelectorAll('.main .row .opt')
                .forEach((el) => {
                    el.classList.remove("active");
                });
            e.classList.add("active");
        });
    });
    showbtn = filter.querySelector('.footer a');

    document.querySelectorAll('.basket-btn').forEach(
        element => {
            element.addEventListener('click', () => {
                fetch(`/basket/addBasket/${element.getAttribute('data-id')}`)
            })
        }
    )
    showbtn.addEventListener('click', ShowBtn);
    document.querySelectorAll('#Pagination button').forEach(
        element => {
            element.addEventListener('click', () => {
                Pagination(element.getAttribute('data-page'))
            })
        }
    )
}
async function ShowBtn () {
    opt = filter.querySelector('.main .row .active');
    if (opt.innerText.trim().toLowerCase() == "all") {
        await fetch(`/shop/index/`)
            .then(response => response.text())
            .then(data => {
                ResetEvent(data);
            })
            .catch(error => console.error('Hata:', error));
    }
    else {
        await fetch(`/shop/Filter/${opt.innerText.trim()}`)
            .then(response => response.text())
            .then(data => {
                ResetEvent(data);
            })
            .catch(error => console.error('Hata:', error));
    }

}

async function Pagination(page) {
    debugger
    if (opt.innerText.trim().toLowerCase() == "all") {
        await fetch(`/shop/index/${page}`)
            .then(response => response.text())
            .then(data => {
                ResetEvent(data);
            })
            .catch(error => console.error('Hata:', error));
    }
    else {
        await fetch(`/shop/Filter/${opt.innerText.trim()}/${page}`)
            .then(response => response.text())
            .then(data => {
                ResetEvent(data);
            })
            .catch(error => console.error('Hata:', error));
    }
}

showbtn.addEventListener('click', ShowBtn );
filter.querySelectorAll('.main .row .opt').forEach(e => {
    e.addEventListener('click',()=>{
        filter.querySelectorAll('.main .row .opt')
        .forEach((el) => {
            el.classList.remove("active");
        });
        e.classList.add("active");
    });
});

document.querySelectorAll('.basket-btn').forEach(
    element => {
        element.addEventListener('click', () => {
            fetch(`/basket/addBasket/${element.getAttribute('data-id')}`)
        })
    }
)

document.querySelectorAll('#Pagination button').forEach(
    element => {
        element.addEventListener('click', () => {
            Pagination(element.getAttribute('data-page'))
        })
    }
)