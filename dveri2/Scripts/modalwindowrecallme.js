$(document).ready(function () { // ��� �a��� �o��� �a������ ���a����
    //��������� ������� �� ������ ������
    $('a.butwritemain').click(function (event) { // �o��� ���� �o ������ 
       
        event.preventDefault(); // ������a�� ��a��a����� �o�� �������a
        $('#overlayL').fadeIn(400, //��a�a�a ��a��o �o�a���a�� ������ �o��o���
		 	function () { //�o��� ���o������ ��������� a���a���
		 	    $('#modal_formL')
					.css('display', 'block') // ����a�� � �o�a���o�o o��a display: none;
					.animate({ opacity: 1, top: '50%' }, 200); // ��a��o ����a����� ��o��a��o��� o��o�������o �o �����a���� ����
		 	   	});
    });
    $.ajax({//������������ ajax �������
        type: 'POST',
        url: '/MegkomnatnyeDveri/BuyDveriModal',
        dataType: 'json',//����� ���� � ������� json
        success: function (data) {
            //������� ����� �������� ��������� � �������
            $('#idbuy').replaceAll(data);
        },
    })
    //��������� ������� �� ������ ������� �������
    $('a.butcallmain').click(function (event) { // �o��� ���� �o ������ � id="go"
        event.preventDefault(); // ������a�� ��a��a����� �o�� �������a
        $('#overlayL').fadeIn(400, //��a�a�a ��a��o �o�a���a�� ������ �o��o���
            function () { //�o��� ���o������ ��������� a���a���
                $('#modal_form_fastbuyL')
                    .css('display', 'block') // ����a�� � �o�a���o�o o��a display: none;
                    .animate({ opacity: 1, top: '50%' }, 200); // ��a��o ����a����� ��o��a��o��� o��o�������o �o �����a���� ����
            });
    });
    $.ajax({//������������ ajax �������
        type: 'POST',
        url: '/MegkomnatnyeDveri/FastBuyDveriModal',
        dataType: 'json',//����� ���� � ������� json
        success: function (data) {
            //������� ����� �������� ��������� � �������
            $('#fastbuy').replaceAll(data);
        },
    })


    /* �a������ �o�a���o�o o��a, ��� ���a�� �o �� �a�o� �o � o��a��o� �o����� */
    $('#modal_closeL, #overlayL, .butcancel').click(function (event) {   // �o��� ���� �o �������� ��� �o��o���
        event.preventDefault();
        $('#modal_formL')
			.animate({ opacity: 0, top: '45%' }, 200,  // ��a��o ������ ��o��a��o��� �a 0 � o��o�������o ����a�� o��o �����
				function () { // �o��� a���a���
				    $(this).css('display', 'none'); // ���a�� ��� display: none;
				    $('#overlayL').fadeOut(400); // �����a�� �o��o���
				}
			);
        $('#modal_form_fastbuyL')
			.animate({ opacity: 0, top: '45%' }, 200,  // ��a��o ������ ��o��a��o��� �a 0 � o��o�������o ����a�� o��o �����
				function () { // �o��� a���a���
				    $(this).css('display', 'none'); // ���a�� ��� display: none;
				    $('#overlayL').fadeOut(400); // �����a�� �o��o���
				}
			);
    });
});