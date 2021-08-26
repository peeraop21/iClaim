<template>
    <div class=" container" align="center">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">ประวัติการใช้สิทธิ์</h2>
                <br>
                <div class="tab-user">
                    <div class="row">
                        <div class="col-3 mt-1 px-0">
                            <img src="@/assets/notify.png" width="75">
                        </div>
                        <div class="col-9 text-start px-1">
                            <!--<span>ชื่อ-สกุล: {{userData.prefix}}{{userData.fname}} {{userData.lname}}</span><br />
                            <span>เลขประจำตัวประชาชน: {{userData.idcardNo}}</span><br />-->
                            <span>เลขที่รับแจ้ง: <span style="color: var(--main-color)">{{accData.accNo}}</span></span><br />
                            <span>ทะเบียนรถที่เกิดเหตุ : <span v-for="(car, index) in accData.car" :key="`car-${index}`" style="color: var(--main-color)">{{car}} </span></span><br />
                            <span>วันที่เกิดเหตุ : <span style="color: var(--main-color)">{{ accData.stringAccDate }}</span></span><br />
                            <span>สิทธิ์ที่ได้รับ: <span style="color: var(--main-color)">35000 บาท</span></span><br />
                            <span>สิทธิ์ที่ใช้ไป: <span style="color: var(--main-color)">25000 บาท</span></span><br />
                            <span>สิทธิ์คงเหลือ: <span style="color: var(--main-color)">10000 บาท</span></span>
                        </div>
                    </div>
                </div>
                <br>
                <br>
                <div align="left" style="width: 100%;">
                    <label>ประวัติการใช้สิทธิ์</label>
                    <br>
                </div>
                <section>
                    <div style="height: 100%; width: 100%;">
                        <div class="accordion" v-for="boto in boto_" :key="boto.id">
                            <div class="accordion-item" :id="'list' + boto.id">
                                <a class="accordion-link" :href="'#list' + boto.id">
                                    <div>
                                        <p>
                                            <ion-icon name="document-text-outline"></ion-icon>{{ boto.boto_no }}
                                            <br>
                                            <ion-icon name="card-outline"></ion-icon>จำนวนเงิน: {{ boto.money }} บาท
                                        </p>
                                    </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer">
                                    <p>
                                        วันที่ใช้สิทธิ์: {{ boto.date }}
                                        <br>
                                        โรงพยาบาลที่รักษา: {{ boto.hospital }}
                                    </p>
                                </div>
                                <div style="text-align: center">
                                    <router-link class="btn-select" :to="{ name: 'RightsHistoryDetail', params: { id: accData.stringAccNo}}">ดูเพิ่มเติม</router-link>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

            </div>
        </div>
        <br>

    </div>
</template>

<script>
    export default {
        name: 'RightsHistory',
        data() {
            return {
                boto_: [
                    {
                        id: 1,
                        boto_no: "บต3/XXX/64/00001",
                        date: "05/06/2564",
                        hospital: "โรงพยาบาลสำโรงการแพทย์",
                        money: 17000
                    },
                    {
                        id: 2,
                        boto_no: "บต3/XXX/64/00002",
                        date: "18/06/2564",
                        hospital: "โรงพยาบาลกรุงเทพ",
                        money: 3000
                    },
                ],
                userData: this.$store.state.userStateData,
                accData: this.$store.getters.accGetter(this.$route.params.id)
            }
        },
        mounted() {
            console.log(this.$store.state.userStateData)
           

        }

    }
</script>

<style>
</style>
