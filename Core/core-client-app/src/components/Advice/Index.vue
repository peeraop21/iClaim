<template>
    <div class="container mt-2">
        <!--<div class="bg-main mt-2">
        <div class="row">
            <div class="col-3 px-0">
                <img src="@/assets/kid.png" width="60" class="px-0 mb-2" style="margin-left: 35%">
            </div>
            <div class="col-9 text-start mt-3 px-3">
                <span style="font-size: 13px; font-weight: bold">นายรำพี พีรำรำ</span>

            </div>
        </div>
    </div>-->
        <!-- <vs-alert :page.sync="page">
        <template #title>
            ขั้นตอนดำเนินการ
        </template>

        <template #page-1>
            1. ลงทะเบียน (ถ้าท่านเคยลงทะเบียนแล้ว จะข้ามขั้นตอนนี้ไป)
        </template>
        <template #page-2>
            2. เลือกรายการแจ้งอุบัติเหตุที่จะใช้สิทธิ์ (ถ้าไม่มีรายการแจ้ง ท่านสามารถเลือกที่แจ้งเหตุใหม่ได้)
        </template>
        <template #page-3>
            3. เลือกประเภทสิทธิ์ที่ได้รับ
            <br />(ค่ารักษาพยาบาล/ค่าสูญเสียอวัยวะ)
        </template>
        <template #page-4>
            4. แนบเอกสารที่เกี่ยวข้องและกรอกข้อมูลเพิ่มเติม
        </template>
        <template #page-5>
            5. ถ่ายภาพหน้าสมุดบัญชีธนาคาร
        </template>
        <template #page-6>
            6. กดส่งคำร้องและยืนยันการส่งด้วย SMS OTP
        </template>
    </vs-alert>-->

        <div align="center" class="mt-4">
            <h2 id="header2">คำแนะนำ</h2>
            <br>
        </div>
        <div class="row mb-2" @click="active=!active" style="margin-top: -10px">
            <div class="col-12">
                <vs-card type="3" align="center">
                    <!-- <template #title>
                    <h3>Pot with a plant</h3>
                </template>-->
                    <template #img>
                        <img src="@/assets/money.jpg" alt="">
                    </template>
                    <template #text>
                        <p style="font-weight: bold;">
                            สิทธิ์คุ้มครองตาม พ.ร.บ.
                        </p>
                    </template>
                </vs-card>
            </div>
        </div>
        <div class="row mb-2" @click="active2=!active2">
            <div class="col-12">
                <vs-card type="3" align="center">
                    <!-- <template #title>
                    <h3>Pot with a plant</h3>
                </template>-->
                    <template #img>
                        <img src="@/assets/search.jpg" alt="">
                    </template>
                    <template #text>
                        <p style="font-weight: bold;">
                            ขั้นตอนดำเนินการ
                        </p>
                    </template>
                </vs-card>
            </div>
        </div>
        <div class="row mb-2" @click="active3=!active3">
            <div class="col-12">
                <vs-card type="3" align="center">
                    <!-- <template #title>
                    <h3>Pot with a plant</h3>
                </template>-->
                    <template #img>
                        <img src="@/assets/id-card.jpg" alt="">
                    </template>
                    <template #text>
                        <p style="font-weight: bold;">
                            เอกสารที่ต้องเตรียม
                        </p>
                    </template>
                </vs-card>
            </div>
        </div>

        <div align="center" class="mt-5">
            <h2 id="header2">กลุ่มผู้ใช้งานที่รองรับ</h2>
            <br>
        </div>
        <div class="row" style="margin-top: -15px">
            <div class="col-6 px-0" align="center">
                <vs-button circle
                           icon
                           light
                           flat
                           :active="active == 1"
                           @click="riderDialog=!riderDialog">
                    <img src="@/assets/rider.png" width="100">
                </vs-button>
                <p style="font-weight: bold; color: var(--main-color);">ผู้ขับขี่รถเอาประกัน</p>
            </div>
            <div class="col-6 px-0" align="center">
                <vs-button circle
                           icon
                           light
                           flat
                           :active="active == 1"
                           @click="passangerDialog=!passangerDialog">
                    <img src="@/assets/passanger.png" width="100">
                </vs-button>
                <p style="font-weight: bold; color: var(--main-color);">ผู้โดยสารรถเอากระกัน</p>
            </div>
        </div>
        <!-- Rider Dialog -->
        <vs-dialog width="550px" not-center v-model="riderDialog">
            <template #header>
                <h4 class="not-margin">
                    ผู้ขับขี่รถเอาประกัน
                </h4>
            </template>
            <div class="con-content" align="left">
                <p>ผู้ขับขี่รถเอาประกัน หมายถึง ผู้ขับขี่รถจักรยานยนต์คันที่ทำประกันภัยไว้ตาม พ.ร.บ. คุ้มครองผู้ประสบภัยจากรถ</p>
            </div>
        </vs-dialog>
        <!-- Passanger Dialog -->
        <vs-dialog width="550px" not-center v-model="passangerDialog">
            <template #header>
                <h4 class="not-margin">
                    ผู้โดยสารรถเอากระกัน
                </h4>
            </template>
            <div class="con-content" align="left">
                <p>ผู้โดยสารรถเอากระกัน หมายถึง ผู้ที่ซ้อนท้ายรถจักรยานยนต์คันที่ทำประกันภัยไว้ตาม พ.ร.บ. คุ้มครองผู้ประสบภัยจากรถ</p>
            </div>
        </vs-dialog>


        <div class="row mt-4 mb-2">
            <div class="col-12">
                <router-link class="btn-next" style="width: 100%; padding: 8px 0;" to="/Accident">{{msg}}</router-link>
            </div>
        </div>

        <!-- Dialog 1 -->
        <vs-dialog width="550px" not-center v-model="active">
            <template #header>
                <h4 class="not-margin">
                    สิทธิ์คุ้มครองตาม พ.ร.บ.
                </h4>
            </template>
            <div class="con-content" align="left">

                <label class="space-title"><strong class="purple-title">ค่าเสียหายเบื่องต้น</strong> ไม่ต้องรอผลพิสูจน์ว่าฝ่ายใดชนหรือฝ่ายใดเป็นฝ่ายถูกหรือผิด</label>
                <br />
                <strong>กรณีผู้ประสบภัยได้รับความเสียหายต่อร่างกาย</strong>
                <p>
                    บริษัทจะจ่ายค่ารักษาพยาบาล ตามจำนวนที่จ่ายไปจริง แต่ไม่เกิน30,000 บาทต่อหนึ่งคน
                </p>
                <strong>กรณีผู้ประสบภัยสูญเสียอวัยวะ/ทุพพลภาพอย่างถาวร</strong>
                <p>
                    บริษัทจะจ่ายค่าเสียหายเบื้องต้น จำนวน 35,000 บาทต่อหนึ่งคน
                </p>
                <strong>กรณีผู้ประสบภัยเสียชีวิต</strong>
                <p>
                    บริษัทจะจ่ายค่าปลงศพตามจำนวนเงินค่าเสียหายเบื้องต้น 35,000 บาทต่อหนึ่งคน
                </p>
                <br />
                <hr>

                <label class="space-title"><strong class="purple-title">ค่าสินไหมทดแทน</strong> คือ เงินชดเชยที่ฝ่ายถูกจะได้รับ หลังการพิสูจน์ผิดถูกแล้ว โดยได้รับความคุ้มครองรวมกับค่าเสียหายเบื้องต้นดังนี้</label>
                <br />
                <strong>กรณีได้รับความเสียหายต่อร่างกาย หรืออนามัย</strong>
                <p>
                    บริษัทจะจ่ายค่ารักษาพยาบาล ตามความเสียหายที่แท้จริงแต่ไม่เกิน80,000  บาทต่อหนึ่งคน
                </p>
                <strong>กรณีสูญเสียอวัยวะ/ทุพพลภาพอย่างถาวร หรือทุพพลภาพถาวรสิ้นเชิง</strong>
                <p>
                    บริษัทจะจ่ายค่าสินไหมทดแทน 200,000 - 500,000 บาทต่อหนึ่งคน
                </p>
                <strong>กรณีเสียชีวิต</strong>
                <p> บริษัทจะชดใช้ค่าสินไหมทดแทน 500,000  บาทต่อหนึ่งคน</p>
                <strong>กรณีที่เข้ารับการรักษาพยาบาลในสถานพยาบาล (คนไข้ใน)</strong>
                <p>
                    บริษัทจะจ่ายค่าชดเชยรายวัน วันละ 200 บาท (จำนวนรวมกันไม่เกิน 20 วัน)
                </p>

            </div>
        </vs-dialog>

        <!-- Dialog 2 -->
        <vs-dialog width="550px" not-center v-model="active2">
            <template #header>
                <h4 class="not-margin">
                    ขั้นตอนดำเนินการ
                </h4>
            </template>
            <div class="con-content" align="left">
                <p>1. ลงทะเบียน (ถ้าเคยลงทะเบียนแล้วจะข้ามขั้นตอนนีี้ไป)</p>
                <p>2. เลือกรายการแจ้งอุบัติเหตุที่จะใช้สิทธิ์ (ถ้าไม่มีรายการแจ้ง ท่านสามารถเลือกที่แจ้งเหตุใหม่ได้)</p>
                <p>3. เลือกประเภทสิทธิ์ที่ได้รับ</p>
                <ul style="margin-top: -15px;">
                    <li>กรณีเบิกค่ารักษาพยาบาล</li>
                    <li>กรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</li>
                </ul>
                <p>4. แนบเอกสารที่เกี่ยวข้องและกรอกข้อมูลเพิ่มเติม</p>
                <p>5. ถ่ายภาพหน้าสมุดบัญชีธนาคาร</p>
                <p>6. กดส่งคำร้องและยืนยันการส่งด้วย SMS OTP</p>
            </div>
        </vs-dialog>
        <!-- Dialog 3 -->
        <vs-dialog width="550px" not-center v-model="active3">
            <template #header>
                <h4 class="not-margin">
                    เอกสารที่ต้องเตรียม
                </h4>
            </template>
            <div class="con-content" align="left">
                <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีบาดเจ็บเบิกค่ารักษาพยาบาล</strong></label>
                <br />
                <p>
                    1. สำเนาบัตรประจำตัวประชาชน<br />
                    2. ใบเสร็จรับเงินค่ารักษาพยาบาล
                </p>
                <br />
                <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีเบิกค่าชดเชย นอนรักษาตัวในโรงพยาบาล (ผู้ป่วยใน)</strong></label>
                <br />
                <p>
                    1. สำเนาบัตรประจำตัวประชาชน<br />
                    2. ใบเสร็จรับเงินค่ารักษาพยาบาล หรือใบสรุปหน้างบ หรือหนังสือรับรองการรักษาตัวเป็นผู้ป่วยในจากโรงพยาบาล
                </p>
                <br />
                <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีทุพพลภาพถาวร หรือสูญเสียอวัยวะ</strong></label>
                <br />
                <p>
                    1. สำเนาบัตรประจำตัวประชาชน<br />
                    2. ใบรับรองแพทย์หรือหลักฐานอื่นที่แสดงว่าผู้นั้นได้รับความเสียหายเพราะประสบภัยจากรถ และหลักฐานรับรองแสดงถึงการสูญเสียอวัยวะ หรือ ทุพพลภาพถาวร เช่น หนังสือรับรองความพิการ<br />
                    3. สำเนาบันทึกประจำวันของพนักงานสอบสวน
                </p>
                <br />
            </div>
        </vs-dialog>

    </div>
</template>
<script>


    export default {
        name: 'Advice',
        data() {
            return {
                msg: "ดำเนินการต่อ",
                active: false,
                active2: false,
                active3: false,
                riderDialog: false,
                passangerDialog: false,
                page: 1
            }
        },
        methods: {
            
        },
        mounted() {
            
        }


    }
</script>
<style>
    .bg-main {
        /*background-color: var(--main-color);
        background: -webkit-linear-gradient(rgb(114 60 177), rgb(144 96 199));*/
        margin-top: -25px;
    }
    /*CSS card and dialog*/
    .vs-card__img {
        width: 50%;
        height: 120px;
        
    }
    .con-footer {
        display: flex; 
        align-items: center;
        justify-content: flex-end;
    }
    /*.col-6.px-0 {
        background-color: white;
    }*/
    .vs-button__content {
        padding: 10px 30px;
    }
    .vs-button {
        margin-left: 10px;
    }
    .not-margin {
        margin: 0px;
        font-weight: normal;
        padding: 10px;
        padding-bottom: 0px;
    }
    
    /*-----*/
    .purple-title {
        color: var(--main-color);
        font-size: 15px;
    }

    .space-title {
        margin: 1.0rem 0rem;
    }

    .space-title-procedure {
        margin: 0rem 0rem 0.1rem 0rem;
    }

    .card-advice-menu {
        border: 2px solid #5c2e91;
        padding: 7% 0px;
        width: 80%;
        margin: 2rem 1rem 2rem 1rem;
        border-radius: 10px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
    }

    .title-advice-menu {
        font-size: 15px;
        color: black;
        margin-bottom: 0px;
    }

    a[class="btn-intro"]:link, a[class="btn-intro"]:visited {
        background-color: white;
        color: black;
        border: 2px solid var(--main-color);
        padding: 7% 20px;
        width: 70%;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }

    a[class="btn-next"]:link, a[class="btn-next"]:visited {
        background-color: var(--main-color);
        color: white;
        padding: 5px 50px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 15px;
    }

</style>