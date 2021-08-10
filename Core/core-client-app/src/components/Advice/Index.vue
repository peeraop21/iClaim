<template>
    <div class="container">
        <!--        <div class="row">
        <div class="col-12">
            <div class="card-advice-menu" v-b-modal.rightsModal>
                <div class="card-body">
                    <p class="title-advice-menu">สิทธิ์คุ้มครองตาม พ.ร.บ.</p>
                </div>

            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            <div class="card-advice-menu" v-b-modal.procedureModal>
                <div class="card-body">
                    <p class="title-advice-menu">
                        ขั้นตอนดำเนินการ
                        <br>และเอกสารที่ต้องเตรียม
                    </p>
                </div>

            </div>
        </div>
    </div>-->
        <div align="center">
            <h2 id="header2">แนะนำสิทธิ์และขั้นตอนการดำเนินการ</h2>
            <br>
        </div>
        <div class="row mb-2 mt-2" @click="active=!active">
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
        <div class="row">
            <div class="col-12">
                <br>
                <br>
                <router-link class="btn-next" to="/Accident">{{msg}}</router-link>
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
                <p>1. สำเนาบัตรประจำตัวประชาชน<br />
                2. ใบเสร็จรับเงินค่ารักษาพยาบาล</p>
                <br />
                <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีเบิกค่าชดเชย นอนรักษาตัวในโรงพยาบาล (ผู้ป่วยใน)</strong></label>
                <br />
                <p>1. สำเนาบัตรประจำตัวประชาชน<br />
                2. ใบเสร็จรับเงินค่ารักษาพยาบาล หรือใบสรุปหน้างบ หรือหนังสือรับรองการรักษาตัวเป็นผู้ป่วยในจากโรงพยาบาล</p>
                <br />
                <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีทุพพลภาพถาวร หรือสูญเสียอวัยวะ</strong></label>
                <br />
                <p>1. สำเนาบัตรประจำตัวประชาชน<br />
                2. ใบรับรองแพทย์หรือหลักฐานอื่นที่แสดงว่าผู้นั้นได้รับความเสียหายเพราะประสบภัยจากรถ และหลักฐานรับรองแสดงถึงการสูญเสียอวัยวะ หรือ ทุพพลภาพถาวร เช่น หนังสือรับรองความพิการ<br />
                3. สำเนาบันทึกประจำวันของพนักงานสอบสวน</p>
                <br />
            </div>
        </vs-dialog>

        <!-- Modal Rights-->
        <b-modal id="rightsModal" title="สิทธิ์คุ้มครองตาม พ.ร.บ." hide-footer>
            <label class="space-title"><strong class="purple-title">ค่าเสียหายเบื่องต้น</strong> ไม่ต้องรอผลพิสูจน์ว่าฝ่ายใดชนหรือฝ่ายใดเป็นฝ่ายถูกหรือผิด</label>
            <br />
            <strong>&emsp;กรณี</strong> ผู้ประสบภัยได้รับความเสียหายต่อร่างกาย บริษัท
            จะจ่ายค่ารักษาพยาบาล ตามจำนวนที่จ่ายไปจริง แต่ไม่เกิน
            30,000 บาทต่อหนึ่งคน
            <br />
            <strong>&emsp;กรณี</strong> ผู้ประสบภัยสูญเสียอวัยวะ/ทุพพลภาพอย่างถาวร
            บริษัทจะจ่ายค่าเสียหายเบื้องต้น จำนวน 35,000 บาทต่อ
            หนึ่งคน
            <br />
            <strong>&emsp;กรณี</strong> ผู้ประสบภัยเสียชีวิต บริษัทจะจ่ายค่าปลงศพตาม
            จำนวนเงินค่าเสียหายเบื้องต้น 35,000 บาทต่อหนึ่งคน
            <br />
            <hr>

            <label class="space-title"><strong class="purple-title">ค่าสินไหมทดแทน</strong> คือ เงินชดเชยที่ฝ่ายถูกจะได้รับ หลังการพิสูจน์ผิดถูกแล้ว โดยได้รับความคุ้มครองรวมกับค่าเสียหายเบื้องต้นดังนี้</label>
            <br />
            <strong>&emsp;กรณี</strong> ได้รับความเสียหายต่อร่างกาย หรืออนามัยบริษัท
            จะจ่ายค่ารักษาพยาบาล ตามความเสียหายที่แท้จริงแต่ไม่เกิน
            80,000  บาทต่อหนึ่งคน
            <br />
            <strong>&emsp;กรณี</strong> สูญเสียอวัยวะ/ทุพพลภาพอย่างถาวร หรือทุพพล
            ภาพถาวรสิ้นเชิง บริษัทจะจ่ายค่าสินไหมทดแทน 200,000 -
            500,000 บาทต่อหนึ่งคน
            <br />
            <strong>&emsp;กรณี</strong> เสียชีวิต บริษัทจะชดใช้ค่าสินไหมทดแทน
            500,000  บาทต่อหนึ่งคน
            <br />
            <strong>&emsp;กรณี</strong> ที่เข้ารับการรักษาพยาบาลในสถานพยาบาล (คน
            ไข้ใน) บริษัทจะจ่ายค่าชดเชยรายวัน วันละ 200 บาท (จำนวน
            รวมกันไม่เกิน 20 วัน)
        </b-modal>


        <!-- Modal Procedure-->
        <b-modal id="procedureModal" title="ขั้นตอนการดำเดินการ และเอกสารที่ต้องเตรียม" hide-footer>
            <label class="space-title-procedure"><strong class="purple-title">ขั้นตอนดำเดินการ</strong></label>
            <br />
            &emsp;1. ลงทะเบียน
            <br />
            &emsp;2. เลือกรับสิทธิ์
            <br />
            &emsp;3. กรอกคำร้อง
            <br />
            &emsp;4. สั่งคำร้อง
            <br />

            <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีบาดเจ็บเบิกค่ารักษาพยาบาล</strong></label>
            <br />
            &emsp;1. สำเนาบัตรประจำตัวประชาชน
            <br />
            &emsp;2. ใบเสร็จรับเงินค่ารักษาพยาบาล
            <br />

            <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีเบิกค่าชดเชย นอนรักษาตัวในโรงพยาบาล (ผู้ป่วยใน)</strong></label>
            <br />
            &emsp;1. สำเนาบัตรประจำตัวประชาชน
            <br />
            &emsp;2. ใบเสร็จรับเงินค่ารักษาพยาบาล หรือใบสรุปหน้างบ หรือหนังสือรับรองการรักษาตัวเป็นผู้ป่วยในจากโรงพยาบาล
            <br />

            <label class="space-title-procedure"><strong class="purple-title">เอกสารประกอบคำร้อง กรณีทุพพลภาพถาวร หรือสูญเสียอวัยวะ</strong></label>
            <br />
            &emsp;1. สำเนาบัตรประจำตัวประชาชน
            <br />
            &emsp;2. ใบรับรองแพทย์หรือหลักฐานอื่นที่แสดงว่าผู้นั้นได้รับความเสียหายเพราะประสบภัยจากรถ และหลักฐานรับรองแสดงถึงการสูญเสียอวัยวะ หรือ ทุพพลภาพถาวร เช่น หนังสือรับรองความพิการ
            <br />
            &emsp;3. สำเนาบันทึกประจำวันของพนักงานสอบสวน
            <br />

        </b-modal>
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
            }
        }

    }
</script>
<style>

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