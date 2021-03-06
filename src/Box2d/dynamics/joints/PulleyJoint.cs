/*******************************************************************************
 * Copyright (c) 2013, Daniel Murphy
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification,
 * are permitted provided that the following conditions are met:
 * 	* Redistributions of source code must retain the above copyright notice,
 * 	  this list of conditions and the following disclaimer.
 * 	* Redistributions in binary form must reproduce the above copyright notice,
 * 	  this list of conditions and the following disclaimer in the documentation
 * 	  and/or other materials provided with the distribution.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
 * INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
 * NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 ******************************************************************************/
/**
 * Created at 12:12:02 PM Jan 23, 2011
 */


using org.jbox2d.common;
using org.jbox2d.pooling;

/**
 * The pulley joint is connected to two bodies and two fixed ground points. The pulley supports a
 * ratio such that: length1 + ratio * length2 <= constant Yes, the force transmitted is scaled by
 * the ratio. Warning: the pulley joint can get a bit squirrelly by itself. They often work better
 * when combined with prismatic joints. You should also cover the the anchor points with static
 * shapes to prevent one side from going to zero length.
 * 
 * @author Daniel Murphy
 */

namespace org.jbox2d.dynamics.joints
{
    public class PulleyJoint : Joint
    {
        public static readonly double MIN_PULLEY_LENGTH = 2.0d;
        private readonly double m_constant;

        private readonly Vec2 m_groundAnchorA = new Vec2();
        private readonly Vec2 m_groundAnchorB = new Vec2();
        private readonly double m_lengthA;
        private readonly double m_lengthB;

        // Solver shared
        private readonly Vec2 m_localAnchorA = new Vec2();
        private readonly Vec2 m_localAnchorB = new Vec2();
        private readonly Vec2 m_localCenterA = new Vec2();
        private readonly Vec2 m_localCenterB = new Vec2();
        private readonly Vec2 m_rA = new Vec2();
        private readonly Vec2 m_rB = new Vec2();
        private readonly double m_ratio;
        private readonly Vec2 m_uA = new Vec2();
        private readonly Vec2 m_uB = new Vec2();
        private double m_impulse;

        // Solver temp
        private int m_indexA;
        private int m_indexB;
        private double m_invIA;
        private double m_invIB;
        private double m_invMassA;
        private double m_invMassB;
        private double m_mass;

        public PulleyJoint(IWorldPool argWorldPool, PulleyJointDef def)
            : base(argWorldPool, def)
        {
            m_groundAnchorA.set(def.groundAnchorA);
            m_groundAnchorB.set(def.groundAnchorB);
            m_localAnchorA.set(def.localAnchorA);
            m_localAnchorB.set(def.localAnchorB);

            m_ratio = def.ratio;

            m_lengthA = def.lengthA;
            m_lengthB = def.lengthB;

            m_constant = def.lengthA + m_ratio*def.lengthB;
            m_impulse = 0.0d;
        }

        public double getLengthA()
        {
            return m_lengthA;
        }

        public double getLengthB()
        {
            return m_lengthB;
        }

        public double getCurrentLengthA()
        {
            Vec2 p = pool.popVec2();
            m_bodyA.getWorldPointToOut(m_localAnchorA, p);
            p.subLocal(m_groundAnchorA);
            double length = p.length();
            pool.pushVec2(1);
            return length;
        }

        public double getCurrentLengthB()
        {
            Vec2 p = pool.popVec2();
            m_bodyB.getWorldPointToOut(m_localAnchorB, p);
            p.subLocal(m_groundAnchorB);
            double length = p.length();
            pool.pushVec2(1);
            return length;
        }


        public Vec2 getLocalAnchorA()
        {
            return m_localAnchorA;
        }

        public Vec2 getLocalAnchorB()
        {
            return m_localAnchorB;
        }


        public override void getAnchorA(Vec2 argOut)
        {
            m_bodyA.getWorldPointToOut(m_localAnchorA, argOut);
        }


        public override void getAnchorB(Vec2 argOut)
        {
            m_bodyB.getWorldPointToOut(m_localAnchorB, argOut);
        }


        public override void getReactionForce(double inv_dt, Vec2 argOut)
        {
            argOut.set(m_uB).mulLocal(m_impulse).mulLocal(inv_dt);
        }


        public override double getReactionTorque(double inv_dt)
        {
            return 0d;
        }

        public Vec2 getGroundAnchorA()
        {
            return m_groundAnchorA;
        }

        public Vec2 getGroundAnchorB()
        {
            return m_groundAnchorB;
        }

        public double getLength1()
        {
            Vec2 p = pool.popVec2();
            m_bodyA.getWorldPointToOut(m_localAnchorA, p);
            p.subLocal(m_groundAnchorA);

            double len = p.length();
            pool.pushVec2(1);
            return len;
        }

        public double getLength2()
        {
            Vec2 p = pool.popVec2();
            m_bodyB.getWorldPointToOut(m_localAnchorB, p);
            p.subLocal(m_groundAnchorB);

            double len = p.length();
            pool.pushVec2(1);
            return len;
        }

        public double getRatio()
        {
            return m_ratio;
        }


        public override void initVelocityConstraints(SolverData data)
        {
            m_indexA = m_bodyA.m_islandIndex;
            m_indexB = m_bodyB.m_islandIndex;
            m_localCenterA.set(m_bodyA.m_sweep.localCenter);
            m_localCenterB.set(m_bodyB.m_sweep.localCenter);
            m_invMassA = m_bodyA.m_invMass;
            m_invMassB = m_bodyB.m_invMass;
            m_invIA = m_bodyA.m_invI;
            m_invIB = m_bodyB.m_invI;

            Vec2 cA = data.positions[m_indexA].c;
            double aA = data.positions[m_indexA].a;
            Vec2 vA = data.velocities[m_indexA].v;
            double wA = data.velocities[m_indexA].w;

            Vec2 cB = data.positions[m_indexB].c;
            double aB = data.positions[m_indexB].a;
            Vec2 vB = data.velocities[m_indexB].v;
            double wB = data.velocities[m_indexB].w;

            Rot qA = pool.popRot();
            Rot qB = pool.popRot();
            Vec2 temp = pool.popVec2();

            qA.set(aA);
            qB.set(aB);

            // Compute the effective masses.
            Rot.mulToOutUnsafe(qA, temp.set(m_localAnchorA).subLocal(m_localCenterA), m_rA);
            Rot.mulToOutUnsafe(qB, temp.set(m_localAnchorB).subLocal(m_localCenterB), m_rB);

            m_uA.set(cA).addLocal(m_rA).subLocal(m_groundAnchorA);
            m_uB.set(cB).addLocal(m_rB).subLocal(m_groundAnchorB);

            double lengthA = m_uA.length();
            double lengthB = m_uB.length();

            if (lengthA > 10d*Settings.linearSlop)
            {
                m_uA.mulLocal(1.0d/lengthA);
            }
            else
            {
                m_uA.setZero();
            }

            if (lengthB > 10d*Settings.linearSlop)
            {
                m_uB.mulLocal(1.0d/lengthB);
            }
            else
            {
                m_uB.setZero();
            }

            // Compute effective mass.
            double ruA = Vec2.cross(m_rA, m_uA);
            double ruB = Vec2.cross(m_rB, m_uB);

            double mA = m_invMassA + m_invIA*ruA*ruA;
            double mB = m_invMassB + m_invIB*ruB*ruB;

            m_mass = mA + m_ratio*m_ratio*mB;

            if (m_mass > 0.0d)
            {
                m_mass = 1.0d/m_mass;
            }

            if (data.step.warmStarting)
            {
                // Scale impulses to support variable time steps.
                m_impulse *= data.step.dtRatio;

                // Warm starting.
                Vec2 PA = pool.popVec2();
                Vec2 PB = pool.popVec2();

                PA.set(m_uA).mulLocal(-m_impulse);
                PB.set(m_uB).mulLocal(-m_ratio*m_impulse);

                vA.x += m_invMassA*PA.x;
                vA.y += m_invMassA*PA.y;
                wA += m_invIA*Vec2.cross(m_rA, PA);
                vB.x += m_invMassB*PB.x;
                vB.y += m_invMassB*PB.y;
                wB += m_invIB*Vec2.cross(m_rB, PB);

                pool.pushVec2(2);
            }
            else
            {
                m_impulse = 0.0d;
            }
//    data.velocities[m_indexA].v.set(vA);
            data.velocities[m_indexA].w = wA;
//    data.velocities[m_indexB].v.set(vB);
            data.velocities[m_indexB].w = wB;

            pool.pushVec2(1);
            pool.pushRot(2);
        }


        public override void solveVelocityConstraints(SolverData data)
        {
            Vec2 vA = data.velocities[m_indexA].v;
            double wA = data.velocities[m_indexA].w;
            Vec2 vB = data.velocities[m_indexB].v;
            double wB = data.velocities[m_indexB].w;

            Vec2 vpA = pool.popVec2();
            Vec2 vpB = pool.popVec2();
            Vec2 PA = pool.popVec2();
            Vec2 PB = pool.popVec2();

            Vec2.crossToOutUnsafe(wA, m_rA, vpA);
            vpA.addLocal(vA);
            Vec2.crossToOutUnsafe(wB, m_rB, vpB);
            vpB.addLocal(vB);

            double Cdot = -Vec2.dot(m_uA, vpA) - m_ratio*Vec2.dot(m_uB, vpB);
            double impulse = -m_mass*Cdot;
            m_impulse += impulse;

            PA.set(m_uA).mulLocal(-impulse);
            PB.set(m_uB).mulLocal(-m_ratio*impulse);
            vA.x += m_invMassA*PA.x;
            vA.y += m_invMassA*PA.y;
            wA += m_invIA*Vec2.cross(m_rA, PA);
            vB.x += m_invMassB*PB.x;
            vB.y += m_invMassB*PB.y;
            wB += m_invIB*Vec2.cross(m_rB, PB);

//    data.velocities[m_indexA].v.set(vA);
            data.velocities[m_indexA].w = wA;
//    data.velocities[m_indexB].v.set(vB);
            data.velocities[m_indexB].w = wB;

            pool.pushVec2(4);
        }


        public override bool solvePositionConstraints(SolverData data)
        {
            Rot qA = pool.popRot();
            Rot qB = pool.popRot();
            Vec2 rA = pool.popVec2();
            Vec2 rB = pool.popVec2();
            Vec2 uA = pool.popVec2();
            Vec2 uB = pool.popVec2();
            Vec2 temp = pool.popVec2();
            Vec2 PA = pool.popVec2();
            Vec2 PB = pool.popVec2();

            Vec2 cA = data.positions[m_indexA].c;
            double aA = data.positions[m_indexA].a;
            Vec2 cB = data.positions[m_indexB].c;
            double aB = data.positions[m_indexB].a;

            qA.set(aA);
            qB.set(aB);

            Rot.mulToOutUnsafe(qA, temp.set(m_localAnchorA).subLocal(m_localCenterA), rA);
            Rot.mulToOutUnsafe(qB, temp.set(m_localAnchorB).subLocal(m_localCenterB), rB);

            uA.set(cA).addLocal(rA).subLocal(m_groundAnchorA);
            uB.set(cB).addLocal(rB).subLocal(m_groundAnchorB);

            double lengthA = uA.length();
            double lengthB = uB.length();

            if (lengthA > 10.0d*Settings.linearSlop)
            {
                uA.mulLocal(1.0d/lengthA);
            }
            else
            {
                uA.setZero();
            }

            if (lengthB > 10.0d*Settings.linearSlop)
            {
                uB.mulLocal(1.0d/lengthB);
            }
            else
            {
                uB.setZero();
            }

            // Compute effective mass.
            double ruA = Vec2.cross(rA, uA);
            double ruB = Vec2.cross(rB, uB);

            double mA = m_invMassA + m_invIA*ruA*ruA;
            double mB = m_invMassB + m_invIB*ruB*ruB;

            double mass = mA + m_ratio*m_ratio*mB;

            if (mass > 0.0d)
            {
                mass = 1.0d/mass;
            }

            double C = m_constant - lengthA - m_ratio*lengthB;
            double linearError = MathUtils.abs(C);

            double impulse = -mass*C;

            PA.set(uA).mulLocal(-impulse);
            PB.set(uB).mulLocal(-m_ratio*impulse);

            cA.x += m_invMassA*PA.x;
            cA.y += m_invMassA*PA.y;
            aA += m_invIA*Vec2.cross(rA, PA);
            cB.x += m_invMassB*PB.x;
            cB.y += m_invMassB*PB.y;
            aB += m_invIB*Vec2.cross(rB, PB);

//    data.positions[m_indexA].c.set(cA);
            data.positions[m_indexA].a = aA;
//    data.positions[m_indexB].c.set(cB);
            data.positions[m_indexB].a = aB;

            pool.pushRot(2);
            pool.pushVec2(7);

            return linearError < Settings.linearSlop;
        }
    }
}