function validateCPF(cpf) {
    cpf = cpf.replace(/[^\d]+/g, '');
    var r = /^(0{11}|1{11}|2{11}|3{11}|4{11}|5{11}|6{11}|7{11}|8{11}|9{11})$/;
    if (!cpf || cpf.length !== 11 || r.test(cpf)) {
        return false;
    }
    function validateDigit(digit) {
        var add = 0;
        var init = digit - 9;
        for (var i = 0; i < 9; i++) {
            add += parseInt(cpf.charAt(i + init)) * (i + 1);
        }
        return (add % 11) % 10 === parseInt(cpf.charAt(digit));
    }
    return validateDigit(9) && validateDigit(10);
};

function validateCPNJ(cnpj) {
    var b = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    cnpj = cnpj.replace(/[^\d]/g, '');

    var r = /^(0{14}|1{14}|2{14}|3{14}|4{14}|5{14}|6{14}|7{14}|8{14}|9{14})$/;
    if (!cnpj || cnpj.length !== 14 || r.test(cnpj)) {
        return false;
    }
    cnpj = cnpj.split('');

    for (var i = 0, n = 0; i < 12; i++) {
        n += cnpj[i] * b[i + 1];
    }
    n = 11 - n % 11;
    n = n >= 10 ? 0 : n;
    if (parseInt(cnpj[12]) !== n) {
        return false;
    }

    for (i = 0, n = 0; i <= 12; i++) {
        n += cnpj[i] * b[i];
    }
    n = 11 - n % 11;
    n = n >= 10 ? 0 : n;
    if (parseInt(cnpj[13]) !== n) {
        return false;
    }
    return true;
}

function ChecaPIS(pis) {

    var ftap = "3298765432";
    var total = 0;
    var i;
    var resto = 0;
    var numPIS = 0;
    var strResto = "";

    numPIS = pis;

    if (numPIS == "" || numPIS == null) {
        return false;
    }

    for (i = 0; i <= 9; i++) {
        resultado = (numPIS.slice(i, i + 1)) * (ftap.slice(i, i + 1));
        total = total + resultado;
    }

    resto = (total % 11)

    if (resto != 0) {
        resto = 11 - resto;
    }

    if (resto == 10 || resto == 11) {
        strResto = resto + "";
        resto = strResto.slice(1, 2);
    }

    if (resto != (numPIS.slice(10, 11))) {
        return false;
    }

    return true;
}

function validaCNS(vlrCNS) {
    // Formulário que contem o campo CNS
    var soma = new Number;
    var resto = new Number;
    var dv = new Number;
    var pis = new String;
    var resultado = new String;
    var tamCNS = vlrCNS.length;
    if ((tamCNS) != 15) {
        return false;
    }
    pis = vlrCNS.substring(0, 11);
    soma = (((Number(pis.substring(0, 1))) * 15) +
        ((Number(pis.substring(1, 2))) * 14) +
        ((Number(pis.substring(2, 3))) * 13) +
        ((Number(pis.substring(3, 4))) * 12) +
        ((Number(pis.substring(4, 5))) * 11) +
        ((Number(pis.substring(5, 6))) * 10) +
        ((Number(pis.substring(6, 7))) * 9) +
        ((Number(pis.substring(7, 8))) * 8) +
        ((Number(pis.substring(8, 9))) * 7) +
        ((Number(pis.substring(9, 10))) * 6) +
        ((Number(pis.substring(10, 11))) * 5));
    resto = soma % 11;
    dv = 11 - resto;
    if (dv == 11) {
        dv = 0;
    }
    if (dv == 10) {
        soma = (((Number(pis.substring(0, 1))) * 15) +
            ((Number(pis.substring(1, 2))) * 14) +
            ((Number(pis.substring(2, 3))) * 13) +
            ((Number(pis.substring(3, 4))) * 12) +
            ((Number(pis.substring(4, 5))) * 11) +
            ((Number(pis.substring(5, 6))) * 10) +
            ((Number(pis.substring(6, 7))) * 9) +
            ((Number(pis.substring(7, 8))) * 8) +
            ((Number(pis.substring(8, 9))) * 7) +
            ((Number(pis.substring(9, 10))) * 6) +
            ((Number(pis.substring(10, 11))) * 5) + 2);
        resto = soma % 11;
        dv = 11 - resto;
        resultado = pis + "001" + String(dv);
    } else {
        resultado = pis + "000" + String(dv);
    }
    if (vlrCNS != resultado) {
        return false;
    } else {
        return true;
    }
}


function validarTitulo(inscricao) {
    var paddedInsc = inscricao;
    var dig1 = 0;
    var dig2 = 0;

    var tam = paddedInsc.length;
    var digitos = paddedInsc.substr(tam - 2, 2);
    var estado = paddedInsc.substr(tam - 4, 2);
    var titulo = paddedInsc.substr(0, tam - 2);
    var exce = (estado == '01') || (estado == '02');
    dig1 = (titulo.charCodeAt(0) - 48) * 9 + (titulo.charCodeAt(1) - 48) * 8 +
        (titulo.charCodeAt(2) - 48) * 7 + (titulo.charCodeAt(3) - 48) * 6 +
        (titulo.charCodeAt(4) - 48) * 5 + (titulo.charCodeAt(5) - 48) * 4 +
        (titulo.charCodeAt(6) - 48) * 3 + (titulo.charCodeAt(7) - 48) * 2;
    var resto = (dig1 % 11);
    if (resto == 0) {
        if (exce) {
            dig1 = 1;
        } else {
            dig1 = 0;
        }
    } else {
        if (resto == 1) {
            dig1 = 0;
        } else {
            dig1 = 11 - resto;
        }
    }

    dig2 = (titulo.charCodeAt(8) - 48) * 4 + (titulo.charCodeAt(9) - 48) * 3 + dig1 * 2;
    resto = (dig2 % 11);
    if (resto == 0) {
        if (exce) {
            dig2 = 1;
        } else {
            dig2 = 0;
        }
    } else {
        if (resto == 1) {
            dig2 = 0;
        } else {
            dig2 = 11 - resto;
        }
    }

    if ((digitos.charCodeAt(0) - 48 == dig1) && (digitos.charCodeAt(1) - 48 == dig2)) {
        return true;
    } else {
        return false;
    }
}


