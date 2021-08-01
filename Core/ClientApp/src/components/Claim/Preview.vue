<template>
    <div class="container space-contianer" align="center">
        <div class="stepper-wrapper">
            <div class="stepper-item completed">
                <div class="step-counter">
                    <ion-icon name="receipt-outline" style="font-size: 20px; color: white"></ion-icon>
                </div>
                <p style="color: #5c2e91">สร้างคำร้อง</p>
            </div>
            <div class="stepper-item completed">
                <div class="step-counter">
                    <ion-icon name="card-outline" style="font-size: 20px; color: white"></ion-icon>
                </div>
                <p style="color: #5c2e91">บัญชีรับเงิน</p>
            </div>
            <div class="stepper-item completed">
                <div class="step-counter">
                    <ion-icon name="send-outline" style="font-size: 20px; color: white"></ion-icon>
                </div>
                <p style="color: #5c2e91">ส่งคำร้อง</p>
            </div>
        </div>
        <h2 id="header2">รายละเอียดคำร้อง</h2>
        <div class="container">

            <!-- คน -->
            <div align="left" style="width: 100%;">
                <ion-icon name="people-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลผู้ประสบภัย</label>
            </div>
            <div class="box-container mb-3">
                <div class="row">
                    <div class="col-9">
                        <p class="mb-0">ชื่อ-สกุล</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-3">
                        <p class="mb-0">อายุ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-3">
                        <p class="mb-0">บ้านเลขที่</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-2">
                        <p class="mb-0">หมู่</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-7">
                        <p class="mb-0">ซอย</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">ถนน</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">ตำบล/แขวง</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">อำเภอ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">จังหวัด</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">รหัสไปรษณีย์</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">เบอร์โทรศัพท์</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
            </div>

            <!-- เหตุ -->
            <div align="left" style="width: 100%;">
                <ion-icon name="bicycle-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลอุบัติเหตุ</label>
            </div>
            <div class="box-container mb-3">
                <div class="row">
                    <div class="col-5">
                        <p class="mb-0">วันที่เกิดเหตุ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-7">
                        <p class="mb-0">ลักษณะเกิดเหตุ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <p class="mb-0">สถานที่เกิดเหตุ</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">รถคันเอาประกันภัย หมายเลขทะเบียน</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">เลขตัวถัง</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">กรมธรรม์คุ้มครองภัยจากรถ เลขที่</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />

            </div>

            <!-- เอกสาร -->
            <div align="left" style="width: 100%;">
                <ion-icon name="reader-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลเอกสารประกอบคำร้อง</label>
            </div>
            <div class="box-container mb-3">
                <p class="mb-0">ลักษณะบาดเจ็บ</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ประเภทผู้ป่วย</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">สำเนาบัตรประจำตัวประชาชน</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ชื่อโรงพยาบาล</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">เลขที่ใบเสร็จ</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">วันที่เข้ารักษา</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>
                <div class="row">
                    <div class="col-6">
                        <p class="mb-0">จำนวนเงิน</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                    <div class="col-6">
                        <p class="mb-0">รวมจำนวนเงินที่ขอเบิก</p>
                        <input type="text" id="input_border_bottom" placeholder="" readonly />
                    </div>
                </div>

            </div>

            <!-- บัญชี -->
            <div align="left" style="width: 100%;">
                <ion-icon name="card-outline" align="left" style="margin-bottom: -5px; padding-right: 5px;"></ion-icon>
                <label align="left">ข้อมูลบัญชีรับเงิน</label>
            </div>
            <div class="box-container mb-3">
                <p class="mb-0">ชื่อธนาคาร</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">ชื่อบัญชีธนาคาร</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
                <p class="mb-0">เลขบัญชีธนาคาร</p>
                <input type="text" id="input_border_bottom" placeholder="" readonly />
            </div>

            <div class="form-check">
                <input class="form-check-input" type="checkbox" v-model="acceptClaim"  >
                <p class="form-check-label" for="flexCheckDefault" style="text-align:start">
                    ข้าพเจ้าตรวจสอบและยืนยันข้อมูลทุกอย่างเป็นความจริง
                </p>
            </div>

            <div v-if="acceptClaim">
                <a class="btn-next" href="\Preview">{{ msg }}</a>
            </div>
        </div>
    </div>
</template>

<script>
    //Your Javascript lives within the Script Tag
    export default {
        data() {
            return {
                acceptClaim: false,
                msg: 'ส่งคำร้อง'
            }
        },

    };
</script>

<style>
    #input_border_bottom {
        width: 100%;
        padding: 0px 5px;
        margin-bottom: 7px;
        box-sizing: border-box;
        border: none;
        border-bottom: 2px solid #ddd;
    }
</style>