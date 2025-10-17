window.drawCircle = function () {
    const canvas = document.getElementById('myCanvas');
    const ctx = canvas.getContext('2d');

    ctx.fillStyle = 'blue';
    ctx.beginPath();
    ctx.arc(250, 150, 50, 0, 2 * Math.PI);
    ctx.fill();
};


window.drawGrid = function (nbCols, nbRows, spriteSize) {
    const canvas = document.getElementById('myCanvas');
    const ctx = canvas.getContext('2d');
    console.log("arkzpodj,zopisqd");
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    ctx.strokeStyle = '#cccccc'; 
    ctx.lineWidth = 1;

    for (let x = 0; x <= nbCols * spriteSize; x += spriteSize) {
        ctx.beginPath();
        ctx.moveTo(x, 0);
        ctx.lineTo(x, nbRows * spriteSize);
        ctx.stroke();
    }

    for (let y = 0; y <= nbRows * spriteSize; y += spriteSize) {
        ctx.beginPath();
        ctx.moveTo(0, y);
        ctx.lineTo(nbCols * spriteSize, y);
        ctx.stroke();
    }
};

window.drawTile = async function (tilePath, x, y, size) {
    const canvas = document.getElementById("myCanvas");
    const ctx = canvas.getContext("2d");

    const img = new Image();
    img.src = tilePath;
    img.onload = () => {
        ctx.drawImage(img, x*size, y*size, size, size);
    };
};

window.eraseTile = function (x, y, size) {
    const canvas = document.getElementById("myCanvas");
    const ctx = canvas.getContext("2d");

    const px = x * size;
    const py = y * size;
    ctx.clearRect(px, py, size, size);
    ctx.fillStyle = '#ffffff';

    ctx.strokeStyle = '#cccccc';
    ctx.lineWidth = 1;


    ctx.fillRect(px, py, size, size);
    ctx.strokeRect(px, py,size, size);
};
document.addEventListener("contextmenu", (e) => {
    if (e.target.id === "myCanvas") e.preventDefault();
});