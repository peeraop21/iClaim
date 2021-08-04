<template>
    <div class="container">
        <div class="row">
            <div class="col-12" align="center">
                <h2 id="header2">ข้อมูลการรับแจ้งเหตุ</h2>
                <br>
                <div class="txt">
                    <p>ชื่อ-สกุล: นายรำพี พีรำรำ</p>
                    <p>เลขประจำตัวประชาชน: 1-2299-00876-32-1</p>
                </div>
                <!--<div class="wrap-collabsible">
                    <input id="collapsible" class="toggle" type="checkbox">
                    <label for="collapsible" class="lbl-toggle">More Info</label>
                    <div class="collapsible-content">
                        <div class="content-inner">
                            <p>
                                QUnit is by calling one of the object that are embedded in JavaScript, and faster JavaScript program could also used with its elegant, well documented, and functional programming using JS, HTML pages Modernizr is a popular browsers without plug-ins. Test-Driven Development.
                            </p>
                        </div>
                    </div>
                </div>-->
                <section>
                    <div style="height: 80%; width: 100%;">
                        <div class="accordion" v-for="accident in accidentsApi" v-bind:key="accident.eaTmpId">
                            <!--v-for="accident in accidents" :key="accident.id"-->
                            <div class="accordion-item" :id="'list' + accident.eaTmpId">
                                <a class="accordion-link" :href="'#list' + accident.eaTmpId">
                                    <div>
                                        <p>
                                            <ion-icon name="newspaper-outline"></ion-icon>เลขที่รับแจ้ง: {{ accident.eaAccNo }}
                                            <br>
                                            <ion-icon name="calendar-outline"></ion-icon>วันที่แจ้งเหตุ: {{ accident.eaAccDate }}
                                        </p>
                                    </div>
                                    <ion-icon name="chevron-down-outline" class="icon ion-md-add"></ion-icon>
                                </a>
                                <div class="answer">
                                    <p>
                                        ทะเบียนรถ:
                                        <label v-for="car in accident.eaCar" v-bind:key="car.eaCarNo"> {{ car.eaCarLicense }},</label>
                                    </p>
                                    <p>
                                        สิทธิ์คงเหลือ: {{ accident.money }} บาท
                                    </p>
                                </div>
                                <div style="text-align: center">
                                    <router-link class="btn-select" to="/Rights">ใช้สิทธิ์</router-link>
                                    <router-link class="btn-checked" to="/CheckStatus">ติดตามสถานะ</router-link>
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
    import axios from 'axios'
    export default {
        name: 'Accident',
        data() {
            return {
                accidentsApi: [],
                accidents: [
                    {
                        id: 1,
                        accident_no: "64/XXX/00001",
                        accident_date: "05/06/2564",
                        license_plare: "บน 7898",
                        money: 10000
                    },
                    {
                        id: 2,
                        accident_no: "64/XXX/00002",
                        accident_date: "29/07/2564",
                        license_plare: "กข 4567",
                        money: 4000
                    },
                    {
                        id: 3,
                        accident_no: "64/XXX/00003",
                        accident_date: "13/08/2564",
                        license_plare: "พย 5643",
                        money: 7000
                    }
                ],

            }
        }, methods: {
            getAccidents() {
                axios.get('/accident')
                    .then((response) => {
                        this.accidentsApi = response.data;
                        console.log(this.accidentsApi);
                    })
                    .catch(function (error) {
                        alert(error);
                    });
            }
        },
        mounted() {
            this.getAccidents();
        }


    }
</script>

<style>
    #header2 {
        font-size: 18px;
        font-weight: bold;
    }

    p.p_right {
        text-align: right;
        width: 90%;
    }

    .txt {
        border-radius: 10px;
        border: 1px solid #cccccc;
        /*height: 300px; */
        padding: 25px 25px 15px 25px;
        width: 100%;
        text-align: left;
        line-height: 15px;
    }

    .mybtn {
        position: center;
        margin-left: 0px;
        justify-content: space-between;
        display: inline-block;
    }

    a[class="btn-select"]:link, a[class="btn-select"]:visited {
        background-color: #5c2e91;
        margin-top: 10px;
        margin-right: 0px;
        width: 130px;
        color: white;
        padding: 3px 10px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 30px;
        /*font-size: 14px;*/
    }

    a[class="btn-select"]:hover, a[class="btn-select"]:active {
        background-color: #50287e;
    }

    a[class="btn-checked"]:link, a[class="btn-checked"]:visited {
        background-color: #d775ff;
        margin-top: 10px;
        margin-left: 10px;
        color: white;
        padding: 3px 15px;
        text-align: center;
        text-decoration: none;
        display: inline-block;
        border-radius: 30px;
        /*font-size: 14px;*/
    }

    a[class="btn-checked"]:hover, a[class="btn-checked"]:active {
        background-color: #a55ac4;
    }

    section {
        width: 100%;
        height: 70%;
        display: flex;
        align-Items: center;
        justify-content: center;
        text-align: left;
    }

    .accordion-item {
        background-color: white;
        border-radius: .4rem;
        border-color: #cccccc;
        margin-bottom: .5rem;
        padding: 1rem 1rem .5rem 1rem;
        border-bottom: 3.5px solid #bbbbbb;
    }

    .accordion-link {
        color: black;
        background-color: white;
        width: 100%;
        display: flex;
        align-Items: center;
        justify-content: space-between;
        padding: 0;
        line-height: 25px;
        text-decoration: none;
    }

        .accordion-link:hover {
            text-decoration: none;
            color: #5c2e91;
        }

        .accordion-link ion-icon {
            color: #5c2e91;
            padding: 0rem .5rem .1rem 0rem;
            vertical-align: middle;
            font-size: 15px;
        }
        /*.accordion-link .ion-md-remove {
        display: none;
    }*/
        .accordion-link .ion-md-add {
            margin-bottom: 5px;
            font-size: 1-px;
        }

    .answer {
        max-height: 0;
        overflow: hidden;
        position: relative;
        background-color: #eeeeee;
        transition: max-height 650ms;
    }

        .answer::before {
            content: "";
            position: absolute;
            width: .3rem;
            height: 100%;
            background-color: #5c2e91;
            top: 50%;
            left: 0;
            transform: translateY(-50%);
        }

        .answer p {
            color: black;
            padding: 0.7rem 0 0 2rem;
        }

    .accordion-item:target .answer {
        max-height: 15rem;
    }

    .accordion-item:target .accordion-link .ion-md-add {
        display: none;
    }

    .box-container {
        border-radius: 10px;
        border: 1px solid #cccccc;
        padding: 20px 20px 15px 20px;
        text-align: left;
    }
</style>
