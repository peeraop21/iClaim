<template>
    <div class="container space-contianer" align="center">
        <h2 id="header2">รายละเอียดคำร้อง</h2>
        <div class="container">

            <!-- คน -->
            <div align="left" style="width: 100%;">
                <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลผู้ประสบภัย</label>
            </diV>
           <div class="box-container mb-3">
                <div class="row">
                    <div class="col-9">
                        <p class="mb-0">ชื่อ-สกุล</p>
                        <input type="text" value="นายรำพี พีรำรำ" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-3">
                        <p class="mb-0">อายุ</p>
                        <input type="text" value="30 ปี" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-3">
                        <p class="mb-0">บ้านเลขที่</p>
                        <input type="text" value="34/32"  id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-2">
                        <p class="mb-0">หมู่</p>
                        <input type="text" value="5" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-7">
                        <p class="mb-0">ซอย</p>
                        <input type="text" value="บ้านหนึ่ง" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">ถนน</p>
                        <input type="text" value="-" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ตำบล/แขวง</p>
                        <input type="text" value="คูขวาง" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">อำเภอ</p>
                        <input type="text" value="ลาดหลุมแก้ว" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">จังหวัด</p>
                        <input type="text" value="ปทุมธานี" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">รหัสไปรษณีย์</p>
                        <input type="text" value="12140" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">เบอร์โทรศัพท์</p>
                        <input type="text" value="0989898989" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
            </div>

            <!-- เหตุ -->
            <div align="left" style="width: 100%;">
                <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลอุบัติเหตุ</label>
            </diV>
            <div class="box-container mb-3">
                <div class="row">
                    <div class="col-5">
                        <p class="mb-0">วันที่เกิดเหตุ</p>
                        <input type="text" value="01/01/2564" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-7">
                        <p class="mb-0">ลักษณะเกิดเหตุ</p>
                        <input type="text" value="รถชนเสาร์ไฟฟ้า" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
                <p class="mb-0">สถานที่เกิดเหตุ</p>
                <input type="text" value="หน้าบริษัทกลาง" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">รถคันเอาประกันภัย หมายเลขทะเบียน</p>
                <input type="text" value="กค 5676" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">เลขตัวถัง</p>
                <input type="text" value="NF125MC-CHSJSKKS21" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                <input type="text" value="05689-56108/กธ/3263688" id="input_border_bottom" placeholder="" readonly/>
           
            </div>

            <!-- เอกสาร -->
            <div align="left" style="width: 100%;">
                <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลเอกสารประกอบคำร้อง</label>
            </diV>
            <div class="box-container mb-3">
                <p class="mb-0">ลักษณะบาดเจ็บ</p>
                <input type="text" value="แผลถลอก" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">ประเภทผู้ป่วย</p>
                <input type="text" value="ผู้ป่วยนอก" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                <input type="text" value="xxxx.jpg" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                <input type="text" value="xxxx.jpg" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">ชื่อโรงพยาบาล</p>
                <input type="text" value="โรงพยาบาลกรุงเทพ" id="input_border_bottom" placeholder="" readonly/>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">เลขที่ใบเสร็จ</p>
                        <input type="text" value="RVP 000234455" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">วันที่เข้ารักษา</p>
                        <input type="text" value="01/01/2564" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">จำนวนเงิน</p>
                        <input type="text" value="10000" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                    <div class="col-6">
                        <p class="mb-0">รวมจำนวนเงินที่ขอเบิก</p>
                        <input type="text" value="10000" id="input_border_bottom" placeholder="" readonly/>
                    </div>
                </div>
               
            </div>

            <!-- บัญชี -->
            <div align="left" style="width: 100%;">
                <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลบัญชีรับเงิน</label>
            </diV>
            <div class="box-container mb-3">
                <p class="mb-0">ชื่อธนาคาร</p>
                <input type="text" value="ธนาคารกรุงเทพ" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                <input type="text" value="นายรำพี พีรำรำ" id="input_border_bottom" placeholder="" readonly/>
                <p class="mb-0">เลขบัญชีธนาคาร</p>
                <input type="text" value="208-098543-4" id="input_border_bottom" placeholder="" readonly/>
            </div>
        </div>
    </div>
</template>