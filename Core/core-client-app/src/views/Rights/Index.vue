<template>
    <div class="container" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">สิทธิ์ที่ได้รับ</h2>
                <br>
                <div align="left" style="width: 100%;">
                    <label class="title-advice-menu">ค่าเสียหายเบื้องต้น</label>
                    <br>
                </div>

                <div>
                    <div class="tab-user px-3" align="left">
                        <div class="row">
                            <div class="col-9">
                                <label class="purple-title" style="font-weight: bold">กรณีเบิกค่ารักษาพยาบาล</label>
                            </div>
                            <div class="col-3" align="right" style="margin-top: -3px">
                                <vs-button circle
                                           icon
                                           primary
                                           flat
                                           @click="Info1=!Info1">
                                    <ion-icon name="information" style="margin: -5px; font-size: 17px"></ion-icon>
                                </vs-button>
                            </div>
                        </div>
                        <p style="line-height: 25px;" class="mt-3">ค่ารักษาพยาบาลตามที่จ่าย โดยบริษัทจะจ่ายค่าเสียหายเบื้องต้นไม่เกิน 30,000 บาทต่อหนึ่งคน</p>
                        <div style="display: flex; justify-content: space-between; align-items: flex-end; width: 100%;">
                            <a v-if="accData.cureRightsBalance >= 0" style="margin-bottom: 7px;">คงเหลือ: {{ accData.cureRightsBalance }} บาท</a>
                            <a v-if="accData.cureRightsBalance < 0" style="margin-bottom: 7px;">คงเหลือ: 0 บาท</a>
                            <br>

                            <router-link v-if="accData.cureRightsBalance != 30000" class="btn-rights-history" :to="{ name: 'RightsHistory', params: { id: accData.stringAccNo, typerights: 1}}">ประวัติการใช้สิทธิ์</router-link>
                            <router-link class="btn-select-rights" :to="{ name: 'ApprovalCreate', params: { id: accData.stringAccNo, type: 1}}">เลือก</router-link>
                        </div>
                    </div>
                    <br>
                </div>
                <div hidden> //Hold ไว้ก่อน
                    <div class="tab-user px-3" align="left" >
                        <div class="row">
                            <div class="col-9">
                                <label class="purple-title" style="font-weight: bold">กรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ</label>
                            </div>
                            <div class="col-3" align="right" style="margin-top: -3px">
                                <vs-button circle
                                           icon
                                           primary
                                           flat
                                           @click="Info2=!Info2">
                                    <ion-icon name="information" style="margin: -5px; font-size: 17px"></ion-icon>
                                </vs-button>
                            </div>
                        </div>
                        <p style="line-height: 25px" class="mt-3">ผู้ประสบภัยที่สูญเสียอวัยวะ/ทุพพลภาพอย่างถาวร บริษัทจะจ่ายค่าเสียหายเบื้องต้น จำนวน 35,000 บาทต่อหนึ่งคน</p>
                        <div style="display: flex; justify-content: space-between; align-items: flex-end; width: 100%;">
                            <a v-if="accData.crippledRightsBalance >= 0" style="margin-bottom: 7px;">คงเหลือ: {{ accData.crippledRightsBalance }} บาท</a>
                            <a v-if="accData.crippledRightsBalance < 0" style="margin-bottom: 7px;">คงเหลือ: 0 บาท</a>
                            <br>

                            <router-link v-if="accData.crippledRightsBalance != 35000" class="btn-rights-history" :to="{ name: 'RightsHistory', params: { id: accData.stringAccNo, typerights: 2}}">ประวัติการใช้สิทธิ์</router-link>
                            <router-link class="btn-select-rights" :to="{ name: 'ApprovalCreate', params: { id: accData.stringAccNo, type: 2}}" hidden>ใช้สิทธิ์</router-link>
                        </div>
                    </div>
                    <br>
                </div>
               

                <!-- Info 1 -->
                <vs-dialog width="550px" not-center v-model="Info1">
                    <template #header>
                        <h4 class="not-margin">
                            ค่ารักษาพยาบาล
                        </h4>
                    </template>
                    <div class="con-content" align="left">
                        <p>ค่ารักษาพยาบาล หมายถึง ค่าใช้จ่ายที่เกิดขึ้นจากการรักษาพยาบาลในกรณีได้รับความเสียหายต่อร่างกาย หรืออนามัย เช่น ค่ายา ค่าเวชภัณฑ์ ค่าอุปกรณ์ทางการแพทย์ เป็นต้น</p>
                    </div>
                </vs-dialog>
                <!-- Info 2 -->
                <vs-dialog width="550px" not-center v-model="Info2">
                    <template #header>
                        <h4 class="not-margin">
                            ค่าสูญเสียอวัยวะ/ทุพพลภาพ
                        </h4>
                    </template>
                    <div class="con-content" align="left">
                        <p>
                            ค่าสูญเสียอวัยวะ/ทุพพลภาพ หมายถึง ค่าเสียหายต่อร่างกาย หรืออนามัยจากอุบัติเหตุ ซึ่งเป็นเหตุให้สูญเสียอวัยวะส่วนหนึ่งส่วนใดของร่างกาย หรือทำให้ทุพพลภาพอย่างถาวร อย่างเช่น <br />
                            <ul>
                                <li>สูญเสียมือ นิ้วมือ แขน ขา เท้า</li>
                                <li>หูหนวก ตาบอด เป็นใบ้หรือสูญเสียความสามารถในการพูด หรือลิ้นขาด สูญเสียอวัยวะสืบพันธุ์ หรือความสามารถในการสืบพันธุ์ จิตพิการอย่างติดตัว</li>
                                <li>สูญเสียอวัยวะอื่นใด ซึ่งส่งผลกระทบต่อการดำรงชีวิต เช่น การสูญเสีย ม้าม ปอด ไต ฟันแท้ทั้งซี่ตั้งแต่ 5 ซี่ขึ้นไป หรือกระโหลกศีรษะถูกทำให้เสียหาย เป็นเหตุให้ต้องใช้กระโหลกเทียม เป็นต้น</li>
                            </ul>
                        </p>
                    </div>
                </vs-dialog>
                <!-- Info 3 -->
                <vs-dialog width="550px" not-center v-model="Info3">
                    <template #header>
                        <h4 class="not-margin">
                            ค่าชดเชยรายวัน
                        </h4>
                    </template>
                    <div class="con-content" align="left">
                        <p>ค่าชดเชยรายวัน หมายถึง ค่าชดเชยที่ผู้ป่วยเข้ารับการรักษาและต้องนอนพักรักษาตัวในสถานพยาบาลตั้งแต่ 6 ชั่วโมงขึ้นไป เช่น ป่วยหนักต้องแอดมิดนอนโรงพยาบาล การเข้าผ่าตัด ฯลฯ และให้รวมถึงการรับตัวไว้เป็นผู้ป่วยในแต่เสียชีวิตก่อน 6 ชั่วโมง</p>
                    </div>
                </vs-dialog>
       
            </div>
    </div>
    <br>
    </div>
</template>

<script>
    import mixin from '../../mixin/index.js'
    export default {
        name: 'Rights',
        mixins: [mixin],
        data() {
            return {
                rights_receives: [
                    {
                        id: 1,
                        medical_expenses: "กรณีเบิกค่ารักษาพยาบาล",
                        detail: "ค่ารักษาพยาบาลตามที่จ่ายจริง โดยจะจ่ายค่าเสียหายเบื้องต้นไม่เกิน 30,000 บาทต่อหนึ่งคน",
                        money: 4000,
                        info: "Info1",

                    },
                    {
                        id: 2,
                        medical_expenses: "กรณีเบิกค่าสูญเสียอวัยวะ/ทุพพลภาพ",
                        detail: "ผู้ประสบภัยสูญเสียอวัยวะ/ทุพพลภาพอย่างถาวรบริษัทจะจ่ายค่าเสียหายเบื้องต้น จำนวน 35,000 บาทต่อหนึ่งคน",
                        money: 8000,
                        info: "Info2"
                    },
                ],
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id),
                claimData: this.$store.getters.ptGetter(this.$route.params.pt),
                Info1: false,
                Info2: false,
                Info3: false,
                

            }
        },
        methods: {                        
        },
        created() {
            this.getJwtToken()
        }
    }
</script>

<style>
    .btn-select-rights {
        box-shadow: 3px 3px 5px -4px #888888;
        background-color: #5c2e91;
        color: white;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 13px;
        padding: 5px 15px;
        border: 2px solid #5c2e91;
    }
    .btn-select-rights:hover {
        background-color: #5c2e91;
        color: white;
        text-decoration: none;
        display: inline-block;
    }
    .btn-rights-history {
        background-color: white;
        color: #5c2e91;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 10px;
        font-size: 13px;
        padding: 5px 10px;
        border: 2px solid #5c2e91;
    }
    .btn-rights-history:hover {
        background-color: white;
        color: #5c2e91;
        text-decoration: none;
        display: inline-block;
    }
</style>
