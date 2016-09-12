  $(document).ready(function () { // ��� �a��� �o��� �a������ ���a����
        //��������� ������� �� ������ ������
      $('a.butbyu').click(function (event) { // �o��� ���� �o ������ � id="go"
		event.preventDefault(); // ������a�� ��a��a����� �o�� �������a
		$('#overlay').fadeIn(400, //��a�a�a ��a��o �o�a���a�� ������ �o��o���
		 	function(){ //�o��� ���o������ ��������� a���a���
				$('#modal_form')
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
        $('a.butcall').click(function (event) { // �o��� ���� �o ������ � id="go"
            event.preventDefault(); // ������a�� ��a��a����� �o�� �������a
            $('#overlay').fadeIn(400, //��a�a�a ��a��o �o�a���a�� ������ �o��o���
                function () { //�o��� ���o������ ��������� a���a���
                    $('#modal_form_fastbuy')
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
    $('#modal_close, #overlay, .butcancel').click(function (event) {   // �o��� ���� �o �������� ��� �o��o���
        event.preventDefault();
		$('#modal_form')
			.animate({opacity: 0, top: '45%'}, 200,  // ��a��o ������ ��o��a��o��� �a 0 � o��o�������o ����a�� o��o �����
				function(){ // �o��� a���a���
					$(this).css('display', 'none'); // ���a�� ��� display: none;
					$('#overlay').fadeOut(400); // �����a�� �o��o���
				}
			);
		$('#modal_form_fastbuy')
			.animate({ opacity: 0, top: '45%' }, 200,  // ��a��o ������ ��o��a��o��� �a 0 � o��o�������o ����a�� o��o �����
				function () { // �o��� a���a���
				    $(this).css('display', 'none'); // ���a�� ��� display: none;
				    $('#overlay').fadeOut(400); // �����a�� �o��o���
				}
			);
	});
});